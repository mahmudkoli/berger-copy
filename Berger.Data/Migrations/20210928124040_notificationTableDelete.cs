using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class notificationTableDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChequeBounceNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClearDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditControlArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomarNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DissChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalesGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Territory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChequeBounceNotification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditLimitCrossNotifiction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditControlArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditLimit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomarNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DissChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Territory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditLimitCrossNotifiction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccasionToCelebrate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomarNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DissChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirsChildDOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondChildDOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SpouseDOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Territory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdChildDOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccasionToCelebrate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentFollowup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomarNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayLimit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DissChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceAge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalesGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Territory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFollowup", x => x.Id);
                });
        }
    }
}
