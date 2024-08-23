using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class repomigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_AspNetUsers_AppUserId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_AppUserId",
                table: "UserProfile");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "UserProfile",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_AppUserId",
                table: "UserProfile",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_AspNetUsers_AppUserId",
                table: "UserProfile",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_AspNetUsers_AppUserId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_AppUserId",
                table: "UserProfile");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "UserProfile",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_AppUserId",
                table: "UserProfile",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_AspNetUsers_AppUserId",
                table: "UserProfile",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
