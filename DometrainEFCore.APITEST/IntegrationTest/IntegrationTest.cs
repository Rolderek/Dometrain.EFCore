using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DometrainEFCore.APITEST.IntegrationTest
{
    public class IntegrationTest
    {
        /*
        private readonly MoviesContext _testContext;
        private readonly MoviesContext _verificationContext;

        public IntegrationTest()
        {
            var options = new DbContextOptionsBuilder<MoviesContext>()
                .UseSqlServer("""
                        Data Source=localhost;
                        Initial Catalog=TestDb;
                        User Id=sa;
                        Password=MySaPassword123;
                        TrustServerCertificate=True;
                        """)
                .Options;

            _testContext = new MoviesContext(options);
            _verificationContext = new MoviesContext(options);

            _testContext.Database.EnsureDeleted();
            _testContext.Database.EnsureCreated();
        }


        [Fact]
        public async Task WhenGenreCreated_GenreIsInDatabase()
        {
            var repository = new GenreRepository(_testContext);
            var genreToCreate = new Genre { Name = "MyAwesomeGenre" };
            var createdGenre = await repository.Create(genreToCreate);

            Assert.NotNull(createdGenre);
            Assert.True(createdGenre.Id > 0);
            var verificationGenre = _verificationContext.Genres.Find(createdGenre.Id);
            Assert.NotNull(verificationGenre);
            Assert.Equal(genreToCreate.Name, verificationGenre.Name);
        }


        */
    }
}
