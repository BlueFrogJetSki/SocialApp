using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileIconMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "Interests",
                table: "UserProfile",
                newName: "IconURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconURL",
                table: "UserProfile",
                newName: "Interests");

            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "UserProfile",
                type: "int",
                nullable: true);
        }
    }
}
