using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class StoryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    ImgURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    AuthorProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_UserProfile_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_StoryId",
                table: "UserProfile",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_AuthorProfileId",
                table: "Story",
                column: "AuthorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Story_StoryId",
                table: "UserProfile",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Story_StoryId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_StoryId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "UserProfile");
        }
    }
}
