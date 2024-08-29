using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class LikeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Post_PostId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_PostId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "UserProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorProfileId = table.Column<int>(type: "int", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Like_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Like_UserProfile_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfileUserProfile",
                columns: table => new
                {
                    FollowersId = table.Column<int>(type: "int", nullable: false),
                    FollowingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileUserProfile", x => new { x.FollowersId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_UserProfileUserProfile_UserProfile_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileUserProfile_UserProfile_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Like_AuthorProfileId",
                table: "Like",
                column: "AuthorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                table: "Like",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileUserProfile_FollowingId",
                table: "UserProfileUserProfile",
                column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "UserProfileUserProfile");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "UserProfile",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_PostId",
                table: "UserProfile",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Post_PostId",
                table: "UserProfile",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
