using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialApp.Migrations
{
    /// <inheritdoc />
    public partial class compositekeysforfollowlike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_UserProfile_AuthorProfileId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_AuthorProfileId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follow",
                table: "Follow");

            migrationBuilder.DropIndex(
                name: "IX_Follow_FollowerId",
                table: "Follow");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Follow");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorProfileId",
                table: "Like",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Follow",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FolloweeId",
                table: "Follow",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                columns: new[] { "AuthorProfileId", "EntityType", "EntityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follow",
                table: "Follow",
                columns: new[] { "FollowerId", "FolloweeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Like_UserProfile_AuthorProfileId",
                table: "Like",
                column: "AuthorProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_UserProfile_AuthorProfileId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follow",
                table: "Follow");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorProfileId",
                table: "Like",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Like",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FolloweeId",
                table: "Follow",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Follow",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Follow",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follow",
                table: "Follow",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Like_AuthorProfileId",
                table: "Like",
                column: "AuthorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowerId",
                table: "Follow",
                column: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_UserProfile_AuthorProfileId",
                table: "Like",
                column: "AuthorProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id");
        }
    }
}
