using Dometrain.EFCore.API.Data.ValueGenerator;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping
{
    public class GenreMapping : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            //az idő nyomonkövetéshez, minden record-hoz az elkészítés idejét adja hozzá
            //builder.Property(genre => genre.CreatedDate).HasDefaultValue(DateTime.Now);
            //ez a felsőt írja át a megadott funcióval és ez lesz az értéke
            //builder.Property(genre => genre.CreatedDate).HasDefaultValueSql("getdate()");
            //harmadik megközelítés, használjuk a ValueGenerator osztályt és azon keresztül adunk értéket neki:
            //builder.Property(genre => genre.CreatedDate).HasValueGenerator<CreatedDateGenerator>();
            //mivel megváltozott ShadowProperty-re ezért a fenti nem lesz jó:
            builder.Property<DateTime>("CreatedDate")
                .HasColumnName("CreatedAt")
                .HasValueGenerator<CreatedDateGenerator>();
            //lefut ha nincs beállítva az érték és ad neki

            builder.ToTable("Genre");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasData(
                new Genre
                {
                    Id = 1,
                    Name = "Drama"
                },
                new Genre
                {
                    Id = 2,
                    Name = "Action"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Comedy"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Sci-fi"
                },
                new Genre
                {
                    Id = 5,
                    Name = "Horror"
                }
            );
        }

    }
}
