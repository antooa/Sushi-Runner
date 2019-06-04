using Microsoft.EntityFrameworkCore.Migrations;

namespace SushiRunner.Data.Migrations
{
    public partial class MyMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MealGroup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MealGroup");
        }
    }
}
