using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class TableRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipeIngredients");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Receipes");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Receipes",
                type: "varchar(95)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ReceipeId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Receipes_AppUserId",
                table: "Receipes",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ReceipeId",
                table: "Ingredients",
                column: "ReceipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients",
                column: "ReceipeId",
                principalTable: "Receipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes");

            migrationBuilder.DropIndex(
                name: "IX_Receipes_AppUserId",
                table: "Receipes");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_ReceipeId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "ReceipeId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Receipes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Receipes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "receipeIngredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ingredient_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    receipe_id = table.Column<int>(type: "int", nullable: false),
                    unit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipeIngredients", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
