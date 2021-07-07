using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class AddSyncDailySalesLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncDailySalesLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Zone = table.Column<string>(maxLength: 150, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 20, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 150, nullable: true),
                    BusinessArea = table.Column<string>(maxLength: 20, nullable: true),
                    CreditControlArea = table.Column<string>(maxLength: 20, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 20, nullable: true),
                    Volume = table.Column<double>(nullable: false),
                    NetAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDailySalesLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncDailySalesLogs");
        }
    }
}
