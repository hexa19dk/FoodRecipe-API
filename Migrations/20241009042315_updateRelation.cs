using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "MealPlanDaysId",
                table: "MealEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealEntries_MealPlanDaysId",
                table: "MealEntries",
                column: "MealPlanDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealEntries_MealPlanDays_MealPlanDaysId",
                table: "MealEntries",
                column: "MealPlanDaysId",
                principalTable: "MealPlanDays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealEntries_MealPlanDays_MealPlanDaysId",
                table: "MealEntries");

            migrationBuilder.DropIndex(
                name: "IX_MealEntries_MealPlanDaysId",
                table: "MealEntries");

            migrationBuilder.DropColumn(
                name: "MealPlanDaysId",
                table: "MealEntries");

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
    }
}
