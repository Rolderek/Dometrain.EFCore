using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Dometrain.EFCore.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GenreController : Controller
    {
        /*
        [HttpGet]
        [ProducesResponseType(typeof(List<Genre>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Movies.ToListAsync()); //módosítva
        }
        */

    }
}

