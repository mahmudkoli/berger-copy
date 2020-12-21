using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerSalesCallUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BPBLActualAMDSales",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<decimal>(
                name: "BPBLActualMTDSales",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BPBLActualMTDSales",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<decimal>(
                name: "BPBLActualAMDSales",
                table: "DealerSalesCalls",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
