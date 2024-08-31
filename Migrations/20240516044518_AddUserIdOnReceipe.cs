using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdOnReceipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Receipes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipes_AppUserId",
                table: "Receipes",
                newName: "IX_Receipes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_AspNetUsers_UserId",
                table: "Receipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipes_AspNetUsers_UserId",
                table: "Receipes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Receipes",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipes_UserId",
                table: "Receipes",
                newName: "IX_Receipes_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipes_AspNetUsers_AppUserId",
                table: "Receipes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
