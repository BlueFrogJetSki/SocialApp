using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class StoryMigrationInUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Story_StoryId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_StoryId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "UserProfile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_StoryId",
                table: "UserProfile",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Story_StoryId",
                table: "UserProfile",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");
        }
    }
}
