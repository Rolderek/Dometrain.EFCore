using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Data.ValueGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Ide jön még egy kis JsonString beállítás a saját converterekhez

/* //példa logger
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//tenant-os swagger:
//builder.Services.AddSwaggerGen(c => c.OperationFilter<TenantHeaderSwaggerAttribute>);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
//
builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();  

// Add a DbContext here
//builder.Services.AddDbContext<MoviesContext>(); //beregisztrálva
//átalakítva a MoviesContext constructor-hoz:
builder.Services.AddDbContext<MoviesContext>(optionsBuilder =>
    {
        var connectionString = builder.Configuration.GetConnectionString("MoviesContext");
        //mivel minden factory method visszatér a builderrel ezlért írhatjuk egybe is
        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine); //loggerFactory használata: .UseLoggerFactory();
            //.EnableSensitiveDataLogging(); //ezt sose használjuk production-ban!!! hasznáűlhatjuk ezek nélkül is
    },
    //még két paramétert megadhatunk, az életciklust amit ajánlott Scoped-re állítani:
    ServiceLifetime.Scoped,
    //a második a connectionString, változik e, mikor változik stb, ebben az esetben sosem fog változni: 
    ServiceLifetime.Singleton);


//DBContext lifetime-hoz használta ezt:
/*
builder.Services.AddDbContextPool<MoviesContext>(optionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("MoviesContext");
    //mivel minden factory method visszatér a builderrel ezlért írhatjuk egybe is
    optionsBuilder
        .UseSqlServer(connectionString);
    //.LogTo(Console.WriteLine); 
};
*/
    


var app = builder.Build();

//ezt ne csináljuk, de azért megmutatta:
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
//context.Database.EnsureDeleted(); 
//context.Database.EnsureCreated(); 
//eldobja a teljes táblát és újra csinálja, de ezek helyett lehet migrálni is:
//await context.Database.MigrateAsync(); //ez nem felel meg a security előírásoknak itt hazsnálva
/*
//nem indítjuk el ha nema legújabb migration-on van a program:
var pendingMigration = await context.Database.GetPendingMigrationsAsync();
if (pendingMigration.Count() > 0)
{
    throw new Exception("Database is not fully migrated.");
}
*/




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
 * --Migration--
 * kell a migrációhoz a efcore.tool, efcore.design a 9.0.17 verziótól
 * dotnet-ef migrations add "neve" migráció hozzáadása/elkezdése
 * dotnet-ef database update - adatbázis update legfrisebb migration verzióra, mugé lehet írni a pontos migráció nevét és akkor arra áll vissza
 * dotnet-ef migrations script > script.txt //migrációk file-ba írása
 * dotnet-ef migrations script Migration2 > script2.txt //egy adott migráció file-ba írása
 * dotnet-ef migrations bundle  //envirorment felépítése Don't forget to copy appsettings.json alongside your bundle if you need it to apply migrations.
 * az előző sok készít egy efbundle.exe file-t 
 * ./efbundle --help //system artifact!
 * 
 * --TEST--
 * inmemory database - lassú lehet a teszteknél
 * fake DbSet-ek
 * test Db - a valós Db-nek egy kisebb verzióját tartalmazza, adatokra vigyázni kell nehogy kikerüljön valami
 * Repository pattern - 
 * 
 * --előnyök--
 * test DB - ugyan az a technológia, nem kell semmi más neki csak egy DB-t cserélni, sok tesztnél érdemes ezt választani
 * memory DB - gyorsabb, mivel a memóriában van, 
 * Repository - gyors, shielded the ORM from domain
 * 
 * 
 * --hátrányok--
 * test DB mindent teszteléni túl sok is lehet, (integrationTest > unitTest), ha változás történik az adatokat újra elő kell állítani a megfelelő formában, tesztek összeférhetetlensége, 
 * memory DB - Missing features, a DB motor miatt, Incompatible data types, Raw SQL nem 100%-ban kompatibilisek egymással (InMemoryDb < SQLite)
 * repository - shielded the ORM from domain ez rossz is plusz interface kell mindenhez ami az ORM-ben van kell plusz osztály +kód
 * 
 * 
 * --teszthez--
 * egy "lebutított" project-et használ
 * efcore.Sqlite nugets
 * MemoryDatabase/SqlLiteTest.cs hazsnálata
 * új solution a project-en belül, valami nem jó a mappában
 * 
 * --Repository Test--
 * 
 * --Integration Test--
 * 
 * --Javaslat--
 * attól függően mit hazsnálunk és mire akarjuk a tesztek hangsúlyát helyezni, úgy válasszuk meg az eszközöket
 * 
 * 
 * --Repository--
 * Query, create, update, delete, minden logika nélkül.
 * 
 * --Tenant/Tenacy-- ilyet nem hazsnálunk
 * csak a saját csoportjának az adatait látja az adatbázisban
 * kétféle megvalósítás lehetséges, 
 * Discriminator -> "lapokra" osztja az adatokat 
 * ConnectionString -> szétbontja az adatbázist több adatbázisra
 * 
 * --Architecture advices-- *clean architecture*
 * web starter project - API
 * assembly - business logic - DOMAIN
 * data projct - EFCore használata - interfaces -> repositories
 * migrations - másik assembly-be
 * infrastructure code ?! interfaces stb...
 * reference -- web -> domain, infrastructure -> domain, data/DB -> domain, migration -> domain
 * data flow -- web -> domain -> infrastructure, domain <-> DB
 * 
 * 
 * 
*/ 