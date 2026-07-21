using Dometrain.EFCore.API.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedStorage.Models;

using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Interceptors
{
    //felülírhatunk eventeket az adatbázisban ezekkel,
    //a klényege nem törli a sort csak törölt státuszba teszi minden lekérdezésnek
    //"logikailag törölve"
    public class SaveChangesInterceptor : ISaveChangesInterceptor //ez egy interface és default 
    {
        public InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context as MoviesContext;
            if (context is null)
            {
                return result;
            }

            var tracker = context.ChangeTracker; //ez a DB-ben egy objektum amin módosítás történt EF oldalról
            //milyen entitások vannak követve most és mi az aktuális állapotuk
            var delteEntries = tracker.Entries<Genre>()//töröltek kigyűjtése
                .Where(entry => entry.State == EntityState.Deleted); //5 féle State van

            foreach (var deleteEntry in delteEntries)
            {
                deleteEntry.Property<bool>("Deleted").CurrentValue = true;
                deleteEntry.State = EntityState.Modified;
                //változás a sorban, deleted flag = true és jöhet a change tracker
            }

            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            return ValueTask.FromResult(SavingChanges(eventData, result));
        }

        //azért használjuk a nem async verziót mert nincs olyan kódunk amihez a másik kellene egyenlőre
        //as null tracking - ne tartsad számon kedves EF törlés és mentésnél
    }
}
