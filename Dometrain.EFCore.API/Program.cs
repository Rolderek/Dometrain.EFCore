using Dometrain.EFCore.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Ide jön még egy kis JsonString beállítás a saját converterekhez
builder.Services.AddControllers()  
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add a DbContext here
builder.Services.AddDbContext<MoviesContext>(); //beregisztrálva

var app = builder.Build();

//ezt ne csináljuk, de azért megmutatta:
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
//context.Database.EnsureDeleted(); 
//context.Database.EnsureCreated(); 
//eldobja a teljes táblát és újra csinálja, de ezek helyett lehet migrálni is:
//await context.Database.MigrateAsync();



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
 * kell a migrációhoz a efcore.tool, efcore.design a 9.0.17 verziótól
 * dotnet-ef migrations add "neve" migráció hozzáadása/elkezdése
 * dotnet-ef database update - adatbázis update legfrisebb migration verzióra, mugé lehet írni a pontos migráció nevét és akkor arra áll vissza
*/