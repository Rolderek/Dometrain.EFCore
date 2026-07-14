using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    Synopsis = table.Column<string>(type: "varchar(max)", nullable: true),
                    AgeRating = table.Column<string>(type: "varchar(32)", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie_Actors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Actors", x => new { x.MovieId, x.Id });
                    table.ForeignKey(
                        name: "FK_Movie_Actors_Pictures_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie_Directors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Directors", x => x.MovieId);
                    table.ForeignKey(
                        name: "FK_Movie_Directors_Pictures_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drama" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Action" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comedy" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sci-fi" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Horror" }
                });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[] { "Id", "AgeRating", "GenreId", "ReleaseDate", "Synopsis", "Title" },
                values: new object[,]
                {
                    { 1, "Adolescent", 1, new DateTime(1999, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "They are one person...", "Fight Club" },
                    { 2, "HighScool", 4, new DateTime(1977, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Awsome!", "Star Wars" }
                });

            migrationBuilder.InsertData(
                table: "Movie_Actors",
                columns: new[] { "Id", "MovieId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 1, "Edward", "Norton" },
                    { 2, 1, "Brad", "Pitt" },
                    { 1, 2, "Mark", "Hamil" },
                    { 2, 2, "Harison", "Ford" }
                });

            migrationBuilder.InsertData(
                table: "Movie_Directors",
                columns: new[] { "MovieId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "David", "Fincher" },
                    { 2, "George", "Lucas" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_GenreId",
                table: "Pictures",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie_Actors");

            migrationBuilder.DropTable(
                name: "Movie_Directors");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
