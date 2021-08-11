using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class ColorBankInstallationTargetColorBankInstallProductivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Target",
                table: "ColorBankInstallationTargets");

            migrationBuilder.AddColumn<int>(
                name: "ColorBankInstallTarget",
                table: "ColorBankInstallationTargets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorBankProductivityTarget",
                table: "ColorBankInstallationTargets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorBankInstallTarget",
                table: "ColorBankInstallationTargets");

            migrationBuilder.DropColumn(
                name: "ColorBankProductivityTarget",
                table: "ColorBankInstallationTargets");

            migrationBuilder.AddColumn<int>(
                name: "Target",
                table: "ColorBankInstallationTargets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
