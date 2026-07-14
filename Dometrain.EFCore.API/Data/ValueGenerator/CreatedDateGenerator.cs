using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Dometrain.EFCore.API.Data.ValueGenerator
{
    public class CreatedDateGenerator : ValueGenerator<DateTime>
    {

        public override DateTime Next(EntityEntry entry)
        {
            return DateTime.UtcNow;
        }


        public override bool GeneratesStableValues => false;

        //nekem ez is kell
        public override bool GeneratesTemporaryValues => false;
    }
}
