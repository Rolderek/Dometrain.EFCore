using Dometrain.EFCore.API.Data.ValueGenerator;
using Dometrain.EFCore.API.Models;
using Dometrain.EFCore.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dometrain.EFCore.API.Services
{
    //itt kötjük össze a UnitOfWork és 

    public interface IBatchGenreService
    {
        Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres);
    }

    public class BatchGenreService : IBatchGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IUnitOfWorkManager _uowManager;

        public BatchGenreService(IGenreRepository repository, IUnitOfWorkManager uowManager)
        {
            _repository = repository;
            _uowManager = uowManager;
        }

        public async Task<IEnumerable<Genre>> CreateGenres(IEnumerable<Genre> genres)
        {
            List<Genre> response = new();
            //mielőtt elkezdem a tranzakckat megnézem hogy mi újság a UnitOfWork-el:
            _uowManager.StartUnitOfWork();

            foreach (var genre in genres)
            {
                response.Add(await _repository.Create(genre));
            }
            //mentem a változásokat a multiple calls, multiple entities miatt ezt használható és minimumon lesz a DB kéréseim száma
            await _uowManager.SaveChangesAsync();
            return response;
        }

        //performance feature: Batch behavior:
        /*
        [HttpPost("batch")]
        [ProducesResponseType(typeof(IEnumerable<Genre>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAll([FromBody] List<Genre> genres)
        {
            var response = await _batchService.CreateGenres(genres);
            return CreatedAtActionResult(nameof(GetAll), response);
        }

        [HttpPost("batch-update")]
        [ProducesResponseType(typeof(IEnumerable<Genre>), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateAll([FromBody] List<Genre> genres)
        {
            var response = await _batchService.UpdateGenres(genres);
            return CreatedAtAction(nameof(GetAll), new { }, response);
        }

        //SQL server profile: (what is happening on that database :)
        //egyenlőre a performance tune-all nem foglalkoztam sokat mivel ennél az appnál nincs még igazi értelme
        //https://learn.microsoft.com/en-us/sql/tools/sql-server-profiler/sql-server-profiler?view=sql-server-ver17
        */
    }
}
