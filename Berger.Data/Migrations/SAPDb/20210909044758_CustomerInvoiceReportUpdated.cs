using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class CustomerInvoiceReportUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PremiumValue",
                table: "CustomerInvoiceReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumValue",
                table: "CustomerInvoiceReports");
        }
    }
}
