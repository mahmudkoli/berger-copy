using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class CreditLimitCrossNotificationadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesOffice",
                table: "ChequeBounceNotifications");

            migrationBuilder.CreateTable(
                name: "CreditLimitCrossNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Depot = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    CustomerNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    PriceGroup = table.Column<string>(nullable: true),
                    CreditControlArea = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<decimal>(nullable: false),
                    TotalDue = table.Column<decimal>(nullable: false),
                    NotificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditLimitCrossNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditLimitCrossNotifications");

            migrationBuilder.AddColumn<string>(
                name: "SalesOffice",
                table: "ChequeBounceNotifications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
