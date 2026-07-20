using Dometrain.EFCore.API.Controllers;
using Dometrain.EFCore.API.Data;
using SharedStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dometrain.EFCore.API.Tests.Controllers
{
    public class MoviesControllerTests
    {
        // segédmetódus, ami minden teszthez egyedi beállításokat (Options) generál
        private DbContextOptions<MoviesContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<MoviesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenMovieDoesNotExist()
        {
            // Arrange
            using var context = new MoviesContext(GetDbContextOptions());
            var controller = new MoviesController(context);

            // Act
            var result = await controller.Get(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkAndMovie_WhenMovieExists()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new MoviesContext(options))
            {
                // Required-ek miatt kell
                var testMovie = new Movie
                {
                    Id = 1,
                    Title = "Test Movie",
                    MainGenreName = "Action",
                    Genre = new Genre { Id = 1, Name = "Action" }
                };
                context.Movies.Add(testMovie);
                await context.SaveChangesAsync();
            }

            // Act - Új context a kontrollernek
            using (var context = new MoviesContext(options))
            {
                var controller = new MoviesController(context);
                var result = await controller.Get(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedMovie = Assert.IsType<Movie>(okResult.Value);
                Assert.Equal("Test Movie", returnedMovie.Title);
            }
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_AndSavesMovie_WithExistingGenre()
        {
            // Arrange - A közös Options tartja össze a memóriabeli adatbázist
            var options = GetDbContextOptions();

            using (var arrangeContext = new MoviesContext(options))
            {
                arrangeContext.Genres.Add(new Genre { Id = 1, Name = "Drama" });
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            using (var controllerContext = new MoviesContext(options))
            {
                var controller = new MoviesController(controllerContext);

                var newMovie = new Movie
                {
                    Id = 2,
                    Title = "New Movie",
                    ReleaseDate = DateTime.Now,
                    MainGenreName = "Drama",
                    Genre = new Genre { Id = 1, Name = "Drama" }
                };

                var result = await controller.Create(newMovie);

                // Assert 1: Helyes HTTP válasz jött?
                var createdResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal(nameof(MoviesController.Get), createdResult.ActionName);
            }

            // Assert 2: Tényleg benne van az adatbázisban?
            using (var assertContext = new MoviesContext(options))
            {
                var movieInDb = await assertContext.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == 2);

                Assert.NotNull(movieInDb);
                Assert.Equal("New Movie", movieInDb.Title);
                Assert.Equal(1, movieInDb.Genre.Id);
            }
        }

        [Fact]
        public async Task Update_ReturnsOk_AndUpdatesProperties()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new MoviesContext(options))
            {
                var originalMovie = new Movie
                {
                    Id = 1,
                    Title = "Old Title",
                    Synopsis = "Old Synopsis",
                    MainGenreName = "Drama",
                    Genre = new Genre { Id = 1, Name = "Drama" }
                };
                context.Movies.Add(originalMovie);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new MoviesContext(options))
            {
                var controller = new MoviesController(context);
                var updatedMovieData = new Movie
                {
                    Title = "New Title",
                    Synopsis = "New Synopsis",
                    MainGenreName = "Drama",
                    Genre = new Genre { Id = 1, Name = "Drama" }
                };

                var result = await controller.Update(1, updatedMovieData);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedMovie = Assert.IsType<Movie>(okResult.Value);

                Assert.Equal("New Title", returnedMovie.Title);
                Assert.Equal("New Synopsis", returnedMovie.Synopsis);
            }

            // Assert DB állapot
            using (var assertContext = new MoviesContext(options))
            {
                var movieInDb = await assertContext.Movies.FindAsync(1);
                Assert.Equal("New Title", movieInDb.Title);
            }
        }

        [Fact]
        public async Task Remove_ReturnsOk_AndDeletesFromDatabase()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new MoviesContext(options))
            {
                var movieToDelete = new Movie
                {
                    Id = 1,
                    Title = "To Be Deleted",
                    MainGenreName = "Action",
                    Genre = new Genre { Id = 2, Name = "Action" }
                };
                context.Movies.Add(movieToDelete);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new MoviesContext(options))
            {
                var controller = new MoviesController(context);
                var result = await controller.Remove(1);

                // Assert
                Assert.IsType<OkResult>(result);
            }

            // Assert DB állapot
            using (var assertContext = new MoviesContext(options))
            {
                var movieInDb = await assertContext.Movies.FindAsync(1);
                Assert.Null(movieInDb);
            }
        }
    }
}