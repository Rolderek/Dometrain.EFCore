using Dometrain.EFCore.API.Controllers;
using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DometrainEFCore.APITEST.FakeDbSet
{
    
    public class FakeDbSetTest
    {
        /*
        [Fact]
        public async Task IfGenreExists_ReturnsGenre()
        {
            var context = CreateFakeDbContext();
            var controller = new GenreController(context);

            var response = await controller.Get(2);
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Action", (okResult.Value as Genre)?.Name);
            await context.DidNotReceive().SaveChangesAsync();
        }

        private MoviesContext CreateFakeDbContext()
        {
            List<Genre>  genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Drama" },
                new Genre { Id = 2, Name = "Action" },
                new Genre { Id = 3, Name = "Comedy" }
            };

            var context = Substitute.For<MoviesContext>();

            var genreSet = genres.AsQueryable().BuildMockDbSet();

            //ide bele írhatom a kívánt eredményeket mint genre az egyéb tesztekhez

            context.Genres.Returns(genreSet);

            return context;

          
        }
        */
    }
}
