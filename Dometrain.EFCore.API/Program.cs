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

//a blazornak engedélyezi kell:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//tenant-os swagger: de mi nem használjuk!
//builder.Services.AddSwaggerGen(c => c.OperationFilter<TenantHeaderSwaggerAttribute>);
builder.Services.AddAuthorization();
//builder.Services.AddControllers(); //a blazoros miatt módosítva erre:
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
//
builder.Services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();  

// Add a DbContext here
//builder.Services.AddDbContext<MoviesContext>(); //beregisztrálva
//átalakítva a MoviesContext constructor-hoz:
builder.Services.AddDbContext<MoviesContext>(optionsBuilder =>
    {
        var connectionString = builder.Configuration.GetConnectionString("MoviesContext");
        //mivel minden factory method visszatér a builderrel ezért írhatjuk egybe is
        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine); //loggerFactory használata: .UseLoggerFactory();
            //.EnableSensitiveDataLogging(); //ezt sose használjuk production-ban!!! hasznáűlhatjuk ezek nélkül is
            //itt még a DB tune-t is be lehet állítani, bér sok értelme ennél a példánál nincsen,
            //pl.: hány update legyen egy saveChanges() metóduson belül
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

//szintén a blazoros megjelenítóhöz kell:
app.UseCors("AllowBlazor");

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
 * dotnet-ef migrations bundle  //envirorment felépítése 
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
 * data project - EFCore használata - interfaces -> repositories
 * migrations - másik assembly-be - az adatbázis séma frissítő file-okat nem ugyan abban a projekt-ben tárolom
 * infrastructure code ?! interfaces stb... logikus öröklődés, projekt függő
 * reference -- web -> domain, infrastructure -> domain, data/DB -> domain, migration -> domain
 * data flow -- web -> domain -> infrastructure, domain <-> DB
 * 
 * 
 * hétfőre - hogyan működik blazorban a DBContext!!!!
 * mi az éles projectben a Blazor server site rendering-et használjuk (egyenlőre olyat még nem csináltam, de fogok)
 * két verziót ismere:
 * A- API készen van egy Db-vel és hozzá minden ami szükséges 
 * (modell, controllel ha kell, program.cs-ben regisztrálva az EFCore dbcontext,
 * migráció, API végpontok HTTP get, put, delete), ezután a blazor projektben WebAssembly-vel készítünk egy 
 * WebAssemblyHostBuilder-t aminek a másik API URL-jeit adjuk meg (HTTP kéréseket)
 * 
 * B- a blazor-ban inject-álunk egy HTTPClient-et és GetFromJsonAsync metódust használunk. 
 * Itt még egyszerűsíteni lehet azzal a dolgot hogy a modelleket mind a két project-nek elérhető,
 * mappába/project-be tessuük.
 * 
 * 
 * a legtöbb teljesítménybeli probléma a rosszul kalibrált EF-ből következik
 * APP(LINQ) -> <-(object, visszafelé a DB-ből)- DbContext -(SQL)-> <-(Results)- DB flissítések DB <-> DB
 * másik gyakori hibalehetőség, több DbContext van és módosítanak egyszerre, ami conflict-hoz vezethet
 * 
 * Slow Queries: Logging system/openTelemetry MSSQL-ben [Query Plan] megmutatja hogy pontosan mit használunk a querire pontosan és az alapján javíthatunk rajta
 * Tuning advisor ha Trace file-t használunk és ez elmondja hogyan gyorsítsunk rajta
 * Ha túl sokszor használunk egy query-t  akkor érdemes compliedQuery-t hazsnálni:
 *  -DbContext, és ami kell még neki bemeneti paraméterként
 *  
 *  
 * --Loading Related Data-- (performance issue)
 * Movies és Actors tábla kapcsolatával, 1 query-vel lekérem a filmeket és egy loop-ban mindhez lekéri
 * később a hozzá kapcoslt actor-okat. 
 * Erre ,megoldás az 
 * "Eager" loading --csak egy lekérés lesz a DB-ből és jönnek vele az actorok is .Include
 * "Explicit" loading -- csak azok az actorok jönnek amikre szükségünk lesz. .Load-al
 * "Lazy" loading --alapértelmezett EF 6.0-tól trigger egy DB query amikor kell.
 * 
 * Eager - All for all M lekéréshez
 * Explicit - All for some M lekéréshez, ez több lekérést jelent, kérdés milyen adatokon futtatom
 * Lazy - csak egy navigation property-hez kell hozzáférnie és megoldja a roundtrip-eket, de sokszor ez nem ajánlott,
 * veszélyes a közvetlen hozzáférés miatt az adatokhoz
 * 
 * --Concurrenccy--
 * 2 thread ugyan azt a record-ot akarja módosítani és lekérni, ilyenkor kérhetem hogy lock-olja a DB a sort,
 * és egymás után hajtsák végre a változtatásokat, úgy hogy a második már az első által változtatott adatokat,
 * kapja meg.
 * vagy "optimistic concurrency" adunk egy oszlopot ami tartalmazza azt hogy melyik tread-nél van most az adat
 * ehhez kell egy byte[] property a model-ben ConcurrencyToken néven és ha valaki változtatni szeretné az
 * adatot, egy ellenőrzéssel megkérdezzük hogy egyezik e a az új token-el és ha nem akkor nem engedjük a 
 * felülírását az adatoknak. (DbUpdateConcurrencyException)
 * 
 * --Mi van ha az adatbázis/séme már létezik?
 * erre lenne példa a DatabaseSecond project, de nem máködik jól a tool ami leképezi az adatbázisból 
 * a modelleket és ami még kell. Otthon megoldom ma (07.20)
 * Másik megközelítés, migrációból is lehet leképezni/módosítja a modelleket "Udpate model from database"
 * jelenleg ez nem elérhető, megoldás, regenerate minden alkalommal amikor változás történik
 * harmadik opció: Log-oljuk a változásokat manuálisan és ezt lekövetjük.
 * LLBLGen pro egy db designer tool ami update-eli ha változás van sajnos ez fizetős
 * Entity developer ennek egy ingyenes változata (LLBLGen)
 * 
 * --partial class--
 * generált kód és kézzel karbantartott kódok naprakészségét hivatott intézni. Pédául:
 * színészeknek egy új fullName adattag ami a két névből áll össze és nem akarom módosítani a teljes
 * adatbázist ez tökéletesen megoldja. 
 * 
 * --kézi mappelés--
 * mégegyszer megnézni ha kész a máködő projekt!
 * SQL Compare - tool használata megoldja hogy ne manuálisan kelljen módosítani
 * 
 * 
 * 
*/ 