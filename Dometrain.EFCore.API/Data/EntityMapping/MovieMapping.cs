using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Dometrain.EFCore.API.Data.EntityMapping
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            //HasQuery filter - amikor bizonyos adatokat nem kell megjelenítened
            builder
                .ToTable("Pictures")
                .HasQueryFilter(movie => movie.ReleaseDate >= new DateTime(1900, 1, 1))
                //itt ez lehetne egy kívülről beállítható változó is, de most beégetett érték 
                .HasKey(movie => movie.Id);

            builder.Property(movie => movie.Title)
                .HasColumnType("varchar")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(movie => movie.ReleaseDate)
                .HasColumnType("date");
            //.HasColumnType("char(23)").HasConversion<string>(); 
            //így string lesz belőle ms-el és kell validátor

            builder.Property(movie => movie.Synopsis)
                .HasColumnType("varchar(max)");

            //az AgeRating-et átalakítjuk string értékké
            builder.Property(movie => movie.AgeRating)
                .HasColumnType("varchar(32)")
                .HasConversion<string>();

            //két adattag egyben megjelnítve: (first és last name)
            //builder.ComplexProperty(movie => movie.Director);
            //builder.OwnsOne(movie => movie.Director); //ezt lehet egyéni entitásként kezelni:
            builder.OwnsOne(movie => movie.Director)
                .ToTable("Movie_Directors");

            //most az Actors adattagot:
            builder.OwnsMany(movie => movie.Actors)
                .ToTable("Movie_Actors");

            /*
            //itt is be lehet állítani a kapcsolatokat
            builder
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies) //itt üresen is hagyhatjuk a zárójelet ha egyértelmű a kapcsolat
                .HasPrincipalKey(genre => genre.Id)
                .HasForeignKey(movie => movie.GenreId);
            */
            /*
            builder
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies)
                .HasForeignKey(movie => movie.GenreId);
            */
            //ez is kell a Genre.Name -> alter key lett miatt:
            
            builder.Property(movie => movie.MainGenreName)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)"); //beleírva  azárójel és a 256
            
            builder 
                .HasOne(movie => movie.Genre)
                .WithMany(genre => genre.Movies)
                .HasPrincipalKey(genre => genre.Name) //ezt is átírni a Genre.Name - alternate key
                .HasForeignKey(movie => movie.MainGenreName);

            //data seed: már nem kell a migráció miatt
            /*
            builder.HasData(new Movie
            {
                Id = 1,
                Title = "Fight Club",
                ReleaseDate = new DateTime(1999, 9, 10),
                Synopsis = "They are one person...",
                GenreId = 1,
                AgeRating = AgeRating.Adolescent
            },
            new Movie
            {
                Id = 2,
                Title = "Star Wars",
                ReleaseDate = new DateTime(1977,08,12),
                Synopsis ="Awsome!",
                GenreId = 4,
                AgeRating = AgeRating.HighScool
            });

            //saját típus
            builder.OwnsOne(movie => movie.Director)
                .HasData(
                    new { MovieId = 1, FirstName = "David", LastName = "Fincher" },
                    new { MovieId = 2, FirstName = "George", LastName = "Lucas" });

            //itt már kell az ID is
            builder.OwnsMany(movie => movie.Actors)
                .HasData(
                    new { MovieId = 1, Id = 1, FirstName = "Edward", LastName = "Norton" },
                    new { MovieId = 1, Id = 2, FirstName = "Brad", LastName = "Pitt" },
                    new { MovieId = 2, Id = 1, FirstName = "Mark", LastName = "Hamil" },
                    new { MovieId = 2, Id = 2, FirstName = "Harison", LastName = "Ford" });
            */


        }


    }
}