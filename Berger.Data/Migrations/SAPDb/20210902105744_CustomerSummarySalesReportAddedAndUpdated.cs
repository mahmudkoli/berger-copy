using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations.SAPDb
{
    public partial class CustomerSummarySalesReportAddedAndUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "volum",
                table: "SAPSalesInfos",
                type: "decimal(18,2)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "fkimg",
                table: "SAPSalesInfos",
                type: "decimal(18,2)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                table: "SAPSalesInfos",
                type: "decimal(18,2)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "fkdat",
                table: "SAPSalesInfos",
                type: "datetime",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Territory",
                table: "QuarterlyPerformanceReports",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SyncTime",
                table: "QuarterlyPerformanceReports",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "SalesGroup",
                table: "QuarterlyPerformanceReports",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Depot",
                table: "QuarterlyPerformanceReports",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "SummaryPerformanceReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "CustomerPerformanceReports");

            migrationBuilder.DropTable(
                name: "SummaryPerformanceReports");

            migrationBuilder.AlterColumn<string>(
                name: "volum",
                table: "SAPSalesInfos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "fkimg",
                table: "SAPSalesInfos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Revenue",
                table: "SAPSalesInfos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "fkdat",
                table: "SAPSalesInfos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Territory",
                table: "QuarterlyPerformanceReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SyncTime",
                table: "QuarterlyPerformanceReports",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "SalesGroup",
                table: "QuarterlyPerformanceReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Depot",
                table: "QuarterlyPerformanceReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
