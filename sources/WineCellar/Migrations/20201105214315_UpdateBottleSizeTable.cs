using Microsoft.EntityFrameworkCore.Migrations;

namespace WineCellar.Migrations
{
    public partial class UpdateBottleSizeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "BottleSizes");

            migrationBuilder.AddColumn<string>(
                name: "BottleSize",
                table: "BottleSizes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BottleSize",
                table: "BottleSizes");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "BottleSizes",
                type: "text",
                nullable: true);
        }
    }
}
