// See https://aka.ms/new-console-template for more information

using DatabaseSecond.Models;
using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();

optionsBuilder.UseSqlServer("Server=.;Database=MoviesDB;Trusted_Connection=True;TrustServerCertificate=True;");

using var context = new MoviesDbContext(optionsBuilder.Options);

var query = context.Movies
    .Where(c => c.Title.StartsWith("S"))
    .Select(c => new { c.Title, c.ReleaseDate, c.Synopsis });

foreach (var movie in query)
{
    Console.WriteLine($"{movie.Title}, {movie.ReleaseDate.ToString()}, {movie.Synopsis}");
}