using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SyncDailySalesLogDistributionChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditControlArea",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "CreditControlAreaName",
                table: "SyncDailySalesLogs");

            migrationBuilder.AddColumn<string>(
                name: "DistributionChannel",
                table: "SyncDailySalesLogs",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributionChannel",
                table: "SyncDailySalesLogs");

            migrationBuilder.AddColumn<string>(
                name: "CreditControlArea",
                table: "SyncDailySalesLogs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditControlAreaName",
                table: "SyncDailySalesLogs",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
