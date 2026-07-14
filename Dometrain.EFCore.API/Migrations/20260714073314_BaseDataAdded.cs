using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    public partial class BaseDataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InternetRating",
                table: "Pictures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, DateTime.Now, "Drama" },
                    { 2, DateTime.Now, "Action" },
                    { 3, DateTime.Now, "Comedy" },
                    { 4, DateTime.Now, "Sci-fi" },
                    { 5, DateTime.Now, "Horror" }
                });

            migrationBuilder.InsertData(
                table: "Pictures",
                columns: new[]
                {
                    "Id",
                    "AgeRating",
                    "GenreId",
                    "ReleaseDate",
                    "Synopsis",
                    "Title",
                    "InternetRating"
                },
                values: new object[,]
                {
                    {
                        1,
                        "Adolescent",
                        1,
                        new DateTime(1999, 9, 10),
                        "They are one person...",
                        "Fight Club",
                        8.8m
                    },
                    {
                        2,
                        "HighScool",
                        4,
                        new DateTime(1977, 8, 12),
                        "Awesome!",
                        "Star Wars",
                        8.6m
                    }
                });

            migrationBuilder.InsertData(
                table: "Movie_Actors",
                columns: new[] { "Id", "MovieId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 1, "Edward", "Norton" },
                    { 2, 1, "Brad", "Pitt" },
                    { 1, 2, "Mark", "Hamill" },
                    { 2, 2, "Harrison", "Ford" }
                });

            migrationBuilder.InsertData(
                table: "Movie_Directors",
                columns: new[] { "MovieId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "David", "Fincher" },
                    { 2, "George", "Lucas" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movie_Actors",
                keyColumns: new[] { "Id", "MovieId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Movie_Actors",
                keyColumns: new[] { "Id", "MovieId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Movie_Actors",
                keyColumns: new[] { "Id", "MovieId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Movie_Actors",
                keyColumns: new[] { "Id", "MovieId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Movie_Directors",
                keyColumn: "MovieId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movie_Directors",
                keyColumn: "MovieId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pictures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pictures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "InternetRating",
                table: "Pictures");
        }
    }
}