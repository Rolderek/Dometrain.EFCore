using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{

    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }


    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Movies.Include(g => g.Genre).ToListAsync()); //módosítva
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var movie = await _context.Movies
            .Include(movie => movie.Genre)
            .FirstOrDefaultAsync(x => x.Id == id);
        //a felsőbe mehet lambda nyugodtan

        //mi van ha több találat is van:
        //var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
        //csak egyet ad vissza a memóriából:           
        //var movie = await _context.Movies.FindAsync(id);

        return movie == null
                ? NotFound()
                : Ok(movie);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);

        await _context.SaveChangesAsync(); //amíg ez nem történi meg csak sorban állnak az adatok
        return CreatedAtAction(nameof(Get), new {id = movie.Id}, movie);
        //mit - GET, melyik id - az új ami létrejott automatikusan, visszaadjuk az egész objektumot ez opcionális
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var exitstingMovie = await _context.Movies.FindAsync(id);
        if (exitstingMovie is null)
            {
                return NotFound();
            }
        exitstingMovie.Title = movie.Title;
        exitstingMovie.ReleaseDate = movie.ReleaseDate;
        exitstingMovie.Synopsis = movie.Synopsis;

        await _context.SaveChangesAsync();
        return Ok(exitstingMovie);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var exitstingMovie = await _context.Movies.FindAsync(id);
        if (exitstingMovie is null)
        {
            return NotFound();
        }
        _context.Movies.Remove(exitstingMovie); //lehet egyszerűbben is írni:
        //csak id-vel is máködik a dolog.
        //_context.Remove(exitstingMovie);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        var filteredTitles = await _context.Movies
            .Where(movie => movie.ReleaseDate.Year == year)
            .Select(movie => new MovieTitle { Id = movie.Id, Title = movie.Title })
            .ToListAsync();

        return Ok(filteredTitles);
        //még így is fetch-igeljük az egész adatot a console-on
    }


    /*
    //régi:
    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        IQueryable<Movie> AllMovies = _context.Movies;
        //vagy var-al is máködik mert az ugyan ez a végeredménynél
        IQueryable<Movie> filteredMovies = AllMovies.Where(m => m.ReleaseDate.Year == year);
        return Ok(await filteredMovies.ToListAsync());
    }
    */

}