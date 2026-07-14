using Dometrain.EFCore.API.Data;
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

            //innen folytatni!!!! 01:56-tól
        }

    }
}
