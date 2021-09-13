using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class QuarterlyPerformAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuarterlyPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SyncTime = table.Column<DateTime>(nullable: false),
                    Depot = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    NoOfBillingDealer = table.Column<int>(nullable: false),
                    MTSValue = table.Column<decimal>(nullable: false),
                    EnamelVolume = table.Column<decimal>(nullable: false),
                    PremiumValue = table.Column<decimal>(nullable: false),
                    PremiumVolume = table.Column<decimal>(nullable: false),
                    DecorativeValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyPerformanceReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuarterlyPerformanceReports");
        }
    }
}
