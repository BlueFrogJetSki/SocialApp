using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class PostAppUserChangedtoUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_AppUserId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_AppUserId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Post");

            //migrationBuilder.DropColumn(
            //    name: "Id",
            //    table: "AspNetUserLogins");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "UserProfile",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorProfileId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Post",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorProfileId = table.Column<int>(type: "int", nullable: true),
                    AuthorProflieId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_UserProfile_AuthorProflieId",
                        column: x => x.AuthorProflieId,
                        principalTable: "UserProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_PostId",
                table: "UserProfile",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AuthorProfileId",
                table: "Post",
                column: "AuthorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorProflieId",
                table: "Comment",
                column: "AuthorProflieId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentId",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_UserProfile_AuthorProfileId",
                table: "Post",
                column: "AuthorProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Post_PostId",
                table: "UserProfile",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_UserProfile_AuthorProfileId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Post_PostId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_PostId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_Post_AuthorProfileId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "AuthorProfileId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Post",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AspNetUserLogins",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Post_AppUserId",
                table: "Post",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_AppUserId",
                table: "Post",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
