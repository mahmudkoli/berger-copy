using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class removefieldfromschemedetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefit",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "RateInDrum",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "TargetVolume",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<string>(
                name: "RateInSKU",
                table: "SchemeDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateInSKU",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<string>(
                name: "Benefit",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateInDrum",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchemeId",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetVolume",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
