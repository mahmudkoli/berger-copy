using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PaymentFollowupNotificationadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentFollowupNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Depot = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: true),
                    InvoiceAge = table.Column<int>(nullable: false),
                    DayLimit = table.Column<int>(nullable: false),
                    PriceGroup = table.Column<string>(nullable: true),
                    NotificationDate = table.Column<DateTime>(nullable: false),
                    IsRprsPayment = table.Column<bool>(nullable: false),
                    IsFastPayCarryPayment = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFollowupNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentFollowupNotifications");
        }
    }
}
