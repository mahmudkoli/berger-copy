using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SyncDailyTargetLogadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncDailyTargetLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
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
                name: "SyncDailyTargetLogs");
        }
    }
}
