using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerSalesCallUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualAMDSales",
                table: "DealerCompetitionSales");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualMTDSales",
                table: "DealerCompetitionSales",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualMTDSales",
                table: "DealerCompetitionSales");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualAMDSales",
                table: "DealerCompetitionSales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
