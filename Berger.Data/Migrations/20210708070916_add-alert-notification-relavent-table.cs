using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addalertnotificationrelaventtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChequeBounceNotification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depot = table.Column<string>(nullable: true),
                    SalesOffice = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    DissChannel = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    ChequeNo = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    ClearDate = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    CreditControlArea = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChequeBounceNotification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditLimitCrossNotifiction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depot = table.Column<string>(nullable: true),
                    SalesOffice = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    DissChannel = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CreditControlArea = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<string>(nullable: true),
                    TotalDue = table.Column<string>(nullable: true),
                    Channel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditLimitCrossNotifiction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccasionToCelebrate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depot = table.Column<string>(nullable: true),
                    SalesOffice = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    DissChannel = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    SpouseDOB = table.Column<DateTime>(nullable: false),
                    ChildDOB = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccasionToCelebrate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentFollowup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depot = table.Column<string>(nullable: true),
                    SalesOffice = table.Column<string>(nullable: true),
                    SalesGroup = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    DissChannel = table.Column<string>(nullable: true),
                    CustomarNo = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<string>(nullable: true),
                    InvoiceAge = table.Column<string>(nullable: true),
                    DayLimit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFollowup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChequeBounceNotification");

            migrationBuilder.DropTable(
                name: "CreditLimitCrossNotifiction");

            migrationBuilder.DropTable(
                name: "OccasionToCelebrate");

            migrationBuilder.DropTable(
                name: "PaymentFollowup");
        }
    }
}
