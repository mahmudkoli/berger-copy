using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class TargetSalesTableDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncDailySalesLogs");

            migrationBuilder.DropTable(
                name: "SyncDailyTargetLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncDailySalesLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BrandCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BusinessArea = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BusinessAreaName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustNo = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    DistributionChannel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DivisionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetAmount = table.Column<double>(type: "float", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SalesGroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SalesOfficeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TerritoryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TerritoryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Volume = table.Column<double>(type: "float", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDailySalesLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncDailyTargetLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BrandCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BusinessArea = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustNo = table.Column<int>(type: "int", nullable: false),
                    DistributionChannel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TargetValue = table.Column<double>(type: "float", nullable: false),
                    TargetVolume = table.Column<double>(type: "float", nullable: false),
                    TerritoryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDailyTargetLogs", x => x.Id);
                });
        }
    }
}
