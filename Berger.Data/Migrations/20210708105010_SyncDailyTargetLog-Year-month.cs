using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SyncDailyTargetLogYearmonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "SyncDailyTargetLogs");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "SyncDailyTargetLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "SyncDailyTargetLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "SyncDailyTargetLogs");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "SyncDailyTargetLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "SyncDailyTargetLogs",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
