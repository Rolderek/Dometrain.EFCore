using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Genre_GenreId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_GenreId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "MainGenreName",
                table: "Pictures",
                type: "varchar(256)",
                nullable: false,
                defaultValue: "Drama");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genre",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Genre_Name",
                table: "Genre",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_MainGenreName",
                table: "Pictures",
                column: "MainGenreName");

            migrationBuilder.Sql("UPDATE Pictures SET MainGenreName = 'Drama'");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Genre_MainGenreName",
                table: "Pictures",
                column: "MainGenreName",
                principalTable: "Genre",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Genre_MainGenreName",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_MainGenreName",
                table: "Pictures");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Genre_Name",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "MainGenreName",
                table: "Pictures");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genre",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_GenreId",
                table: "Pictures",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Genre_GenreId",
                table: "Pictures",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
