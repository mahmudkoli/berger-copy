using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UniverseReachAnalysis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UniverseReachAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    BusinessArea = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    FiscalYear = table.Column<string>(nullable: true),
                    OutletNumber = table.Column<int>(nullable: false),
                    DirectCovered = table.Column<int>(nullable: false),
                    IndirectCovered = table.Column<int>(nullable: false),
                    DirectTarget = table.Column<int>(nullable: false),
                    IndirectTarget = table.Column<int>(nullable: false),
                    IndirectManual = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniverseReachAnalysis", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniverseReachAnalysis");
        }
    }
}
