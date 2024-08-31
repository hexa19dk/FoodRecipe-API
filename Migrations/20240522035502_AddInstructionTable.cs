using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_UserId",
                table: "Receipes");

            migrationBuilder.DropIndex(
                name: "IX_Receipes_UserId",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "PreparationSteps",
                table: "Receipes");

            migrationBuilder.RenameColumn(
                name: "ReceipeId",
                table: "Ingredients",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_ReceipeId",
                table: "Ingredients",
                newName: "IX_Ingredients_RecipeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Receipes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Receipes",
                type: "varchar(95)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Receipes_AppUserId",
                table: "Receipes",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
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
                name: "FK_Ingredients_Receipes_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes");

            migrationBuilder.DropIndex(
                name: "IX_Receipes_AppUserId",
                table: "Receipes");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Receipes");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Ingredients",
                newName: "ReceipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                newName: "IX_Ingredients_ReceipeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Receipes",
                type: "varchar(95)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PreparationSteps",
                table: "Receipes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Receipes_UserId",
                table: "Receipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_ReceipeId",
                table: "Ingredients",
                column: "ReceipeId",
                principalTable: "Receipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_AspNetUsers_UserId",
                table: "Receipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
