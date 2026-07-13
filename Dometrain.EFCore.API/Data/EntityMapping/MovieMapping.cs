using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Dometrain.EFCore.API.Data.EntityMapping
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder
                .ToTable("Pictures")
                .HasKey(movie => movie.Id);

            builder.Property(movie => movie.Title)
                .HasColumnType("varchar")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(movie => movie.ReleaseDate)
                .HasColumnType("date");

            builder.Property(movie => movie.Synopsis)
                .HasColumnType("varchar(max)");

            /*
            //itt is be lehet állítani a kapcsolatokat
            builder
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies) //itt üresen is hagyhatjuk a zárójelet ha egyértelmű a kapcsolat
                .HasPrincipalKey(genre => genre.Id)
                .HasForeignKey(movie => movie.GenreId);
            */
            builder
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies)
                .HasForeignKey(movie => movie.GenreId);

            //data seed:
            builder.HasData(new Movie
            {
                Id = 1,
                Title = "Fight Club",
                ReleaseDate = new DateTime(1999, 9, 10),
                Synopsis = "They are one person...",
                GenreId = 1
            });
        }

        
    }
}
