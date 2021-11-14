using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class KPIPerformanceReportcreditcontrolarearemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditControlArea",
                table: "KpiPerformanceReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreditControlArea",
                table: "KpiPerformanceReports",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
