using Microsoft.EntityFrameworkCore.Migrations;

namespace WineCellar.Migrations
{
    public partial class InitVarietalsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData( "Varietals", "Varietal", "Chardonnay" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Cabernet Sauvignon" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Red Table Wine" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "White Table Wine" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Merlot" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Sauvignon Blanc" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Pinot Noir" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Chenin Blanc" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Champagne" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Sparklineg Wine" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Rose" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Pinot Grigio" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Gamay Beaujolia" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Riesling" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Zinfandel" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "White Zinfandel" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Barolo" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Gewurztramier" );
            migrationBuilder.InsertData( "Varietals", "Varietal", "Chianti" );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Chardonnay" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Cabernet Sauvignon" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Red Table Wine" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "White Table Wine" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Merlot" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Sauvignon Blanc" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Pinot Noir" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Chenin Blanc" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Champagne" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Sparklineg Wine" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Rose" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Pinot Grigio" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Gamay Beaujolia" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Riesling" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Zinfandel" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "White Zinfandel" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Barolo" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Gewurztramier" );
            migrationBuilder.DeleteData( "Varietals", "Varietal", "Chianti" );
        }
    }
}
