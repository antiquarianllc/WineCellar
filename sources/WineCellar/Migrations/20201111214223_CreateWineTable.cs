using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WineCellar.Migrations
{
    public partial class CreateWineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Vintage = table.Column<string>(nullable: true),
                    WhenPurchased = table.Column<string>(nullable: true),
                    BottlesPurchased = table.Column<int>(nullable: false),
                    BottlesDrank = table.Column<int>(nullable: false),
                    BottleSizeEntityId = table.Column<int>(nullable: false),
                    VarietalEntityId = table.Column<int>(nullable: false),
                    WineryEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wines_BottleSizes_BottleSizeEntityId",
                        column: x => x.BottleSizeEntityId,
                        principalTable: "BottleSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wines_Varietals_VarietalEntityId",
                        column: x => x.VarietalEntityId,
                        principalTable: "Varietals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wines_Wineries_WineryEntityId",
                        column: x => x.WineryEntityId,
                        principalTable: "Wineries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_BottleSizeEntityId",
                table: "Wines",
                column: "BottleSizeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_VarietalEntityId",
                table: "Wines",
                column: "VarietalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_WineryEntityId",
                table: "Wines",
                column: "WineryEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wines");
        }
    }
}
