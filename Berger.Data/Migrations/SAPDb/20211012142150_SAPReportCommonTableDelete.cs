using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class SAPReportCommonTableDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryWisePerformanceReports");

            migrationBuilder.DropTable(
                name: "ColorBankPerformanceReports");

            migrationBuilder.DropTable(
                name: "CustomerInvoiceReports");

            migrationBuilder.DropTable(
                name: "CustomerPerformanceReports");

            migrationBuilder.DropTable(
                name: "KpiPerformanceReports");

            migrationBuilder.DropTable(
                name: "QuarterlyPerformanceReports");

            migrationBuilder.DropTable(
                name: "SummaryPerformanceReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryWisePerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerClassification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryWisePerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ColorBankPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditControlArea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorBankPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoiceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DistributionChannel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceNoOrBillNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PremiumValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesOffice = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KpiPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerClassification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuarterlyPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DecorativeValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EnamelVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MTSValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    NoOfBillingDealer = table.Column<int>(type: "int", nullable: false),
                    PremiumValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummaryPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditControlArea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Depot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SalesGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryPerformanceReports", x => x.Id);
                });
        }
    }
}
