using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SyncSetupupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailySyncCount",
                table: "SyncSetups");

            migrationBuilder.AddColumn<int>(
                name: "SyncHourlyInterval",
                table: "SyncSetups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncHourlyInterval",
                table: "SyncSetups");

            migrationBuilder.AddColumn<int>(
                name: "DailySyncCount",
                table: "SyncSetups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
