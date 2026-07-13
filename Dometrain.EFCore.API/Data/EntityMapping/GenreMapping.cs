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
