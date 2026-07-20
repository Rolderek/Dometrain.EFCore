using SharedStorage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping

{
    public class ActorMapping : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            /*
            // a kapcsolat manuális beállítása általunk létrehozott entity segítségével:
            builder.HasMany(actor => actor.Movies)
                .WithMany(Movie => movie.Actors)
                .UsingEntity("Movie_Actor",
                left => left.HasOne(typeof(Movie))
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .HasPrincipalKey(nameof(Movie.Identifier))
                    .HasConstraintName("FK_MovieActor_Movie")
                    .OnDelete(DeleteBehavior.Cascade))
                right => right.HasOne(typeof(Actor))
                    .WithMany()
                    .HasForeignKey("ActorId")
                    .HasPrincipalKey(nameof(Actor.Id))
                    .HasConstraintName("FK_MovieActor_Actor")
                    .OnDelete(DeleteBehavior.Cascade),
                linkBuilder => linkBuilder.HasKey("MovieId", "ActorId")
            );


            */

            //létrehoz egy link identity-t, ha átírom jöhet a migration és jó lesz
            
        }
    }
}
