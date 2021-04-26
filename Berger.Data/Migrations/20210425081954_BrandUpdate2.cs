using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class BrandUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGallon",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "IsKG",
                table: "BrandInfos");

            migrationBuilder.AddColumn<bool>(
                name: "IsLiquid",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPowder",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLiquid",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "IsPowder",
                table: "BrandInfos");

            migrationBuilder.AddColumn<bool>(
                name: "IsGallon",
                table: "BrandInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsKG",
                table: "BrandInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
