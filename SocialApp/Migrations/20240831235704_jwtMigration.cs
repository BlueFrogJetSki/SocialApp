using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class jwtMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_UserProfile_AuthorProflieId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AuthorProflieId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AuthorProflieId",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorProfileId",
                table: "Comment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorProfileId",
                table: "Comment",
                column: "AuthorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_UserProfile_AuthorProfileId",
                table: "Comment",
                column: "AuthorProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_UserProfile_AuthorProfileId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AuthorProfileId",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorProfileId",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorProflieId",
                table: "Comment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorProflieId",
                table: "Comment",
                column: "AuthorProflieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_UserProfile_AuthorProflieId",
                table: "Comment",
                column: "AuthorProflieId",
                principalTable: "UserProfile",
                principalColumn: "Id");
        }
    }
}
