using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlterMealPlanTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEntries_MealPlanDays_MealPlanDaysId",
                table: "MealEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MealEntries_Recipes_RecipeId",
                table: "MealEntries");

            migrationBuilder.DropIndex(
                name: "IX_MealEntries_MealPlanDaysId",
                table: "MealEntries");

            migrationBuilder.DropIndex(
                name: "IX_MealEntries_RecipeId",
                table: "MealEntries");

            migrationBuilder.DropColumn(
                name: "MealPlanDayId",
                table: "MealEntries");

            migrationBuilder.DropColumn(
                name: "MealPlanDaysId",
                table: "MealEntries");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealEntries");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "MealEntries");

            migrationBuilder.AddColumn<int>(
                name: "MealPlanDayId",
                table: "MealPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealPlans",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "MealPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_RecipeId",
                table: "MealPlans",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_Recipes_RecipeId",
                table: "MealPlans",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_Recipes_RecipeId",
                table: "MealPlans");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_RecipeId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealPlanDayId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "MealPlans");

            migrationBuilder.AddColumn<int>(
                name: "MealPlanDayId",
                table: "MealEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MealPlanDaysId",
                table: "MealEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealEntries",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "MealEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_MealPlanDaysId",
                table: "MealEntries",
                column: "MealPlanDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_RecipeId",
                table: "MealEntries",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEntries_MealPlanDays_MealPlanDaysId",
                table: "MealEntries",
                column: "MealPlanDaysId",
                principalTable: "MealPlanDays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEntries_Recipes_RecipeId",
                table: "MealEntries",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
