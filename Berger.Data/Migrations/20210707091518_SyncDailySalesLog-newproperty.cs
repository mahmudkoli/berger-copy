using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SyncDailySalesLognewproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountGroup",
                table: "SyncDailySalesLogs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandCode",
                table: "SyncDailySalesLogs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessAreaName",
                table: "SyncDailySalesLogs",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditControlAreaName",
                table: "SyncDailySalesLogs",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustNo",
                table: "SyncDailySalesLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "SyncDailySalesLogs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "SyncDailySalesLogs",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesGroupName",
                table: "SyncDailySalesLogs",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOffice",
                table: "SyncDailySalesLogs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOfficeName",
                table: "SyncDailySalesLogs",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountGroup",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "BrandCode",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "BusinessAreaName",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "CreditControlAreaName",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "CustNo",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "SalesGroupName",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "SalesOffice",
                table: "SyncDailySalesLogs");

            migrationBuilder.DropColumn(
                name: "SalesOfficeName",
                table: "SyncDailySalesLogs");
        }
    }
}
