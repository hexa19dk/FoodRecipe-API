using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesToReceipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriesID",
                table: "Receipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipes_CategoriesID",
                table: "Receipes",
                column: "CategoriesID");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_Categories_CategoriesID",
                table: "Receipes",
                column: "CategoriesID",
                principalTable: "Categories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_Categories_CategoriesID",
                table: "Receipes");

            migrationBuilder.DropIndex(
                name: "IX_Receipes_CategoriesID",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "CategoriesID",
                table: "Receipes");
        }
    }
}
