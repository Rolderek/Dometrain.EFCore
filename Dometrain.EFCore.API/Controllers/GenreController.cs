using Dometrain.EFCore.API.Data;
using SharedStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Dometrain.EFCore.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GenreController : Controller
    {

        private readonly MoviesContext _context;

        public GenreController(MoviesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Genre>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(await _context.Genres.ToListAsync());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Genre), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            return genre == null ? NotFound() : Ok(genre);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Genre), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = genre.Id }, genre);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Genre genre)
        {
            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre == null)
            {
                return NotFound();
            }
            existingGenre.Name = genre.Name;
            await _context.SaveChangesAsync();
            return Ok(existingGenre);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var existingGenre = await _context.Genres.FindAsync(id);
            if (existingGenre is null)
            {
                return NotFound();
            }
            _context.Genres.Remove(existingGenre);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //átírva ide a Repository-ból:
        [HttpGet("names")] //csak a név ami látszik a swagger UI-on
        [ProducesResponseType(typeof(IEnumerable<GenreName>), StatusCodes.Status200OK)] //mivel trünk vissza
        public async Task<IActionResult> GetNames()
        {
            var names = await _context.Database
                .SqlQuery<GenreName>($"SELECT Name From Genre")
                .ToListAsync();

            return Ok(names);
        }

        //alulról a második metódus a Repository-ból, csak adatot ad vissza meghatározott typust/fomrátumot
        [HttpGet("GetFromCustomIdAndNameNotComedy/{id:int}")]//ne felejtsem ez a bemeneti paramétert!
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)] //itt is a pontos visszatérési érték kell
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFromQuery([FromRoute] int id)
        {
            var genreNames = await _context.Genres
            .FromSql($"SELECT * FROM Genre WHERE Id >= {id}")
            .Where(genre => genre.Name != "Comedy")
            .Select(genre => genre.Name) //csak a neveket adja vissza, szűrés
            .ToListAsync();

             if (!genreNames.Any())
             {
                 return NotFound();
             }

             return Ok(genreNames);
        }


        //új metódus:
        /*
        [HttpGet("from-query")]
        [ProducesResponseType(typeof(IEnumerable<Genre>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFromQuery()
        {
           
        }
        */

        
    }
}

