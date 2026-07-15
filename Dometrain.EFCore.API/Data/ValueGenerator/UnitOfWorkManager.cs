namespace Dometrain.EFCore.API.Data.ValueGenerator
{

    public interface IUnitOfWorkManager
    {
        void StartUnitOfWork();
        bool IsUnitOfWorkStarted { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWorkManager :IUnitOfWorkManager
    {
        //ugyan azt a HTTP requestet hazsnáljuk a program futása során
        private readonly MoviesContext _context;

        public UnitOfWorkManager(MoviesContext context)
        {
            _context = context;
        }

        private bool _isUnitOfWorkStarted = false;
        public void StartUnitOfWork() 
        {
            _isUnitOfWorkStarted = true;   
        }

        public bool IsUnitOfWorkStarted => _isUnitOfWorkStarted;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }

    //ezt az osztályt hazsnálhatnánk a GenreRepository-hoz ha lenne ilyenünk mint a példában
}
