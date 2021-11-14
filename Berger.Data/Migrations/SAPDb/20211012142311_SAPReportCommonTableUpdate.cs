using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class SAPReportCommonTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryWisePerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Zone = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 200, nullable: true),
                    CustomerClassification = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryWisePerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ColorBankPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Zone = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    CreditControlArea = table.Column<string>(maxLength: 50, nullable: true),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorBankPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoiceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesOffice = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Zone = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    DistributionChannel = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 200, nullable: true),
                    InvoiceNoOrBillNo = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Time = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CustomerNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 200, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KpiPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 200, nullable: true),
                    CustomerClassification = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuarterlyPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    NoOfBillingDealer = table.Column<int>(nullable: false),
                    MTSValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnamelVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PremiumVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DecorativeValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyPerformanceReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummaryPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    SyncTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Depot = table.Column<string>(maxLength: 50, nullable: true),
                    SalesGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Territory = table.Column<string>(maxLength: 50, nullable: true),
                    Zone = table.Column<string>(maxLength: 50, nullable: true),
                    Division = table.Column<string>(maxLength: 50, nullable: true),
                    CreditControlArea = table.Column<string>(maxLength: 50, nullable: true),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TillDateVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryPerformanceReports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
