using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Receipes_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Receipes_RecipeId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_Categories_CategoriesID",
                table: "Receipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receipes",
                table: "Receipes");

            migrationBuilder.RenameTable(
                name: "Receipes",
                newName: "Recipes");

            migrationBuilder.RenameColumn(
                name: "CategoriesID",
                table: "Recipes",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipes_CategoriesID",
                table: "Recipes",
                newName: "IX_Recipes_CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipes_AppUserId",
                table: "Recipes",
                newName: "IX_Recipes_AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Instructions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Ingredients",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Recipes_RecipeId",
                table: "Instructions",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_AppUserId",
                table: "Recipes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoriesId",
                table: "Recipes",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Recipes_RecipeId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Recipes_RecipeId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_AppUserId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoriesId",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Receipes");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "Receipes",
                newName: "CategoriesID");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_CategoriesId",
                table: "Receipes",
                newName: "IX_Receipes_CategoriesID");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_AppUserId",
                table: "Receipes",
                newName: "IX_Receipes_AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Instructions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Ingredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receipes",
                table: "Receipes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Receipes_RecipeId",
                table: "Ingredients",
                column: "RecipeId",
                principalTable: "Receipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Receipes_RecipeId",
                table: "Instructions",
                column: "RecipeId",
                principalTable: "Receipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_Categories_CategoriesID",
                table: "Receipes",
                column: "CategoriesID",
                principalTable: "Categories",
                principalColumn: "ID");
        }
    }
}
