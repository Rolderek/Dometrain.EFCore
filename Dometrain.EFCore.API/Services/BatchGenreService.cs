using Dometrain.EFCore.API.Data.ValueGenerator;
using Dometrain.EFCore.API.Models;
using Dometrain.EFCore.API.Repositories;

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
    }
}
