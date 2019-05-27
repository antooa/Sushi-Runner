using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SushiRunner.Data.Migrations
{
    public partial class MyMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Courier");

            migrationBuilder.DropIndex(
                name: "IX_Order_CourierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MealGroup");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultAddress",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DefaultAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<long>(
                name: "CourierId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MealGroup",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Courier",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CourierId",
                table: "Order",
                column: "CourierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Courier_CourierId",
                table: "Order",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
