using Microsoft.EntityFrameworkCore.Migrations;

namespace WineCellar.Migrations
{
    public partial class InitBottleSizeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "BottleSizes",
                columns: new string[ ] { "BottleSize", "Volume", "Default" },
                values: new object[ ] { "375", "ml", false } );
            migrationBuilder.InsertData(
                 table: "BottleSizes",
                 columns: new string[ ] { "BottleSize", "Volume", "Default" },
                 values: new object[ ] { "750", "ml", true } );
            migrationBuilder.InsertData(
                 table: "BottleSizes",
                 columns: new string[ ] { "BottleSize", "Volume", "Default" },
                 values: new object[ ] { "1.5", "L", false } );
            migrationBuilder.InsertData(
                 table: "BottleSizes",
                 columns: new string[ ] { "BottleSize", "Volume", "Default" },
                 values: new object[ ] { "3.0", "L", false } );
            migrationBuilder.InsertData(
                 table: "BottleSizes",
                 columns: new string[ ] { "BottleSize", "Volume", "Default" },
                 values: new object[ ] { "5.0", "L", false } );
            migrationBuilder.InsertData(
                 table: "BottleSizes",
                 columns: new string[ ] { "BottleSize", "Volume", "Default" },
                 values: new object[ ] { "6.0", "L", false } );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "375" );
            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "750" );
            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "1.5" );
            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "3.0" );
            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "5.0" );
            migrationBuilder.DeleteData( "BottleSizes", "BottleSize", "6.0" );

        }
    }
}
