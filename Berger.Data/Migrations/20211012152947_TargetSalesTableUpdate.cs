using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class TargetSalesTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncDailySalesLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Zone = table.Column<string>(maxLength: 150, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 20, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 150, nullable: true),
                    BusinessArea = table.Column<string>(maxLength: 20, nullable: true),
                    BusinessAreaName = table.Column<string>(maxLength: 250, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 20, nullable: true),
                    SalesGroupName = table.Column<string>(maxLength: 250, nullable: true),
                    SalesOfficeName = table.Column<string>(maxLength: 250, nullable: true),
                    SalesOffice = table.Column<string>(maxLength: 20, nullable: true),
                    AccountGroup = table.Column<string>(maxLength: 20, nullable: true),
                    BrandCode = table.Column<string>(maxLength: 20, nullable: true),
                    Division = table.Column<string>(maxLength: 20, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 250, nullable: true),
                    CustNo = table.Column<int>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    NetAmount = table.Column<double>(nullable: false),
                    DistributionChannel = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDailySalesLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncDailyTargetLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Zone = table.Column<string>(maxLength: 150, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 20, nullable: true),
                    BusinessArea = table.Column<string>(maxLength: 20, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 20, nullable: true),
                    SalesOffice = table.Column<string>(maxLength: 20, nullable: true),
                    AccountGroup = table.Column<string>(maxLength: 20, nullable: true),
                    BrandCode = table.Column<string>(maxLength: 20, nullable: true),
                    Division = table.Column<string>(maxLength: 20, nullable: true),
                    CustNo = table.Column<int>(nullable: false),
                    TargetVolume = table.Column<double>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    DistributionChannel = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDailyTargetLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncDailySalesLogs");

            migrationBuilder.DropTable(
                name: "SyncDailyTargetLogs");
        }
    }
}
