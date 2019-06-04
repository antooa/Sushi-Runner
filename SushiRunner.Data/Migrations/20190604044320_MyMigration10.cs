using Microsoft.EntityFrameworkCore.Migrations;

namespace SushiRunner.Data.Migrations
{
    public partial class MyMigration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comment",
                newName: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                newName: "IX_Comment_AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_AspNetUsers",
                table: "Comment",
                column: "AspNetUsers",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_AspNetUsers",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "AspNetUsers",
                table: "Comment",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AspNetUsers",
                table: "Comment",
                newName: "IX_Comment_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
