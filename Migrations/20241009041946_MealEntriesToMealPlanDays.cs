using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class MealEntriesToMealPlanDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealEntriesId",
                table: "MealPlanDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanDays_MealEntriesId",
                table: "MealPlanDays",
                column: "MealEntriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanDays_MealEntries_MealEntriesId",
                table: "MealPlanDays",
                column: "MealEntriesId",
                principalTable: "MealEntries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanDays_MealEntries_MealEntriesId",
                table: "MealPlanDays");

            migrationBuilder.DropIndex(
                name: "IX_MealPlanDays_MealEntriesId",
                table: "MealPlanDays");

            migrationBuilder.DropColumn(
                name: "MealEntriesId",
                table: "MealPlanDays");
        }
    }
}
