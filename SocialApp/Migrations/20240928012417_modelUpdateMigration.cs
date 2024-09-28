using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class modelUpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_CommentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Comment_CommentId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Post_PostId",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_CommentId",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_PostId",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comment",
                newName: "parentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CommentId",
                table: "Comment",
                newName: "IX_Comment_parentCommentId");

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "Post",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubcommentCount",
                table: "Comment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_parentCommentId",
                table: "Comment",
                column: "parentCommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_parentCommentId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "SubcommentCount",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "parentCommentId",
                table: "Comment",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_parentCommentId",
                table: "Comment",
                newName: "IX_Comment_CommentId");

            migrationBuilder.AddColumn<string>(
                name: "CommentId",
                table: "Like",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "Like",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Comment",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Like_CommentId",
                table: "Like",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_PostId",
                table: "Like",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_CommentId",
                table: "Comment",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Comment_CommentId",
                table: "Like",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Post_PostId",
                table: "Like",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }
    }
}
