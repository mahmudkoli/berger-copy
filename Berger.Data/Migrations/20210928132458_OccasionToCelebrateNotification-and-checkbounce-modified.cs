using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class OccasionToCelebrateNotificationandcheckbouncemodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChequeBounceNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Depot = table.Column<string>(nullable: true),
                    SalesOffice = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    ChequeNo = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    NotificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChequeBounceNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccasionToCelebrateNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Depot = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    SpouseDOB = table.Column<DateTime>(nullable: true),
                    FirsChildDOB = table.Column<DateTime>(nullable: true),
                    SecondChildDOB = table.Column<DateTime>(nullable: true),
                    ThirdChildDOB = table.Column<DateTime>(nullable: true),
                    NotificationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccasionToCelebrateNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChequeBounceNotifications");

            migrationBuilder.DropTable(
                name: "OccasionToCelebrateNotifications");
        }
    }
}
