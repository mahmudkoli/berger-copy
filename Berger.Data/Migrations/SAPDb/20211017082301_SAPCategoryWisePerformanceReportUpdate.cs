using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class SAPCategoryWisePerformanceReportUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistributionChannel",
                table: "CategoryWisePerformanceReports",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributionChannel",
                table: "CategoryWisePerformanceReports");
        }
    }
}
