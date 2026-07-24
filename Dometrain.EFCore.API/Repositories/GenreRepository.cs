using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Data.ValueGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedStorage.Models;

namespace Dometrain.EFCore.API.Repositories
{
    /*
    * itt megvalósítva az interface amit használ az osztály később, ez az egész csak egy bemutató "mocking"
    * elveszi a Unit of work koncepciót, mert mindig ezt a repository-t fogod hívni és nem a hagyományo utat ha jól értem
    * ha átállítjuk a controller osztályokat egy ilyen repository hazsnálatára az "await _context.SaveChangesAsync();
    * minden felhazsnáló számára elérhető(látható) és hiába fut alatta egy DBContext elérik a sensitive adatokat a Repon keresztül
    * Mivel a repo hívások félúton elhasalhatnak az adatbázist egy "inconsistent" állapotban tudják hagyni.
    * 
    */
    public interface IGenreRepository
    {
        //ide tudunk még hozzáadni ha szükséges 
        //ezek a feladatok amiket megvalósítunk a DbContext-ben is a teljesség igénye nélkül:
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre?> Get(int id);
        Task<Genre> Create(Genre genre);
        Task<Genre?> Update(int id, Genre genre);
        Task<bool> Delete(int id);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly MoviesContext _context;
        private readonly IUnitOfWorkManager _uowManager;

        public GenreRepository(MoviesContext context, IUnitOfWorkManager uowManager)
        {
            _context = context;
            _uowManager = uowManager;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            if (!_uowManager.IsUnitOfWorkStarted)
            {
                await _context.SaveChangesAsync();
            }
            
            return genre;
        }

        public async Task<bool> Delete(int id)
        {
            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre is null)
            {
                return false;
            }
            _context.Genres.Remove(existingGenre);
            if (!_uowManager.IsUnitOfWorkStarted)
            {
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public Task<Genre?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Genre>> GetAll() 
        {
            var genres = await _context.Genres.ToListAsync();
            return genres;
        }

        public async Task<Genre?> Update(int id, Genre genre)
        {
            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre != null)
            {
                existingGenre.Name = genre.Name;
                await _context.SaveChangesAsync();
                return existingGenre;
            }
            return null;
        }

        public async Task<IEnumerable<Genre>> GetAllFromQuery() //még nincs meghívva a controllerben
        {
            var minimumGenreId = 2;
            
            var genres = await _context.Genres
                //raw SQL parancsot tudunk beadni ezzel:
                .FromSql($"SELECT * FROM Genre WHERE Genre.Id >= {minimumGenreId}")
                //ez a FromSql még összeköthető linkel is, szépen kombinálható
                .Where(genre => genre.Name != "Comdey")
                .ToListAsync();
            var genre2 = await _context.Genres.Where(g => g.Name != "Comedy" && g.Id >= minimumGenreId).ToListAsync();

            return genre2;
        }

        //ez csak egy eredményt ad vissza:
        public async Task<IEnumerable<GenreName>> GetNames()
        {
            //var names = await _context.GenreNames.ToListAsync(); //vagy ha nem akarok ennek is DbSet-et csináni:
            var names = await _context.Database
                .SqlQuery<GenreName>($"SELECT Name From Genre")
                .ToListAsync();
            return names;


        }

    }

    //shadowproperty és queryfilter egyben, sajnos ez sincs bekötve:
    //logikai törlés
    public class GenreMappingVersionTwo : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property<DateTime>("CreatedData")
                .HasColumnName("CreatedAt")
                .HasValueGenerator<CreatedDateGenerator>();

            builder.Property(g => g.Name)
                .HasMaxLength(256)
                .HasColumnType("varchar");

            builder.Property<bool>("Deleted") //shadow property, modelben nincs csak az adatbázisban
                .HasDefaultValue(false);

            builder //global queryFilter figyeli a logikát
                .HasQueryFilter(g => EF.Property<bool>(g, "Deleted") == false)
                .HasAlternateKey(g => g.Name);
        }
    }
}
