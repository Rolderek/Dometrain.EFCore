using Dometrain.EFCore.API.Data.EntityMapping;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data
{
    public class MoviesContext : DbContext
    {



        //public DbSet<Movie> Movies { get; set; } = null!; //nem a leg elegánsabb megoldás, de elmegy
        public DbSet<Movie> Movies => Set<Movie>();
        //így már szép és hatákonyabb is, a get only property a protected Set metódust fogja használni

        public DbSet<Genre> Genres => Set<Genre>();

        /*
        //ez futtató környezet függvényében változhat és konfigurálni kell, erre van egy megoldás
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //a vedeóban docker-t használ, de ez is működik
            optionsBuilder.UseSqlServer(
                "Server=.;Database=MoviesDB;Trusted_Connection=True;TrustServerCertificate=True;");
            optionsBuilder.LogTo(Console.WriteLine); //mit és hogyna csinálja, írja ki a console-ra
        }
        */
        //ez lett a fölötte lévő kód helyett, mivel a base osztályon keresztül megkapja az előtző metódusban foglaltakat, de át kell írni a Program.cs-t hogy ez rendesen működjön
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {

        }


        //itt átírhatjuk az adatbázis konvenciókat

        protected override void OnModelCreating(ModelBuilder modelBuilder) //ez elég kaotikus tud lenni sok property-nél
        {
            //ez az egész átment a MovieMapping osztályba
            /*
            modelBuilder.Entity<Movie>()
                .ToTable("Pictures")
                .HasKey(movie => movie.Id);

            modelBuilder.Entity<Movie>().Property(movie => movie.Title)
                .HasColumnType("varchar")
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Movie>().Property(movie => movie.ReleaseDate)
                .HasColumnType("date");

            modelBuilder.Entity<Movie>().Property(movie => movie.Synopsis)
                .HasColumnType("varchar(max)")
                .HasColumnName("Plot");
            */

            modelBuilder.ApplyConfiguration(new MovieMapping());
            //ezt lehagytam:
            modelBuilder.ApplyConfiguration(new GenreMapping());

        }

        public object DidNotReceive()
        {
            throw new NotImplementedException();
        }
    }
    
}
