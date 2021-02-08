using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class BrandInfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCBInstalled",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMTS",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCBInstalled",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "IsMTS",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "BrandInfos");
        }
    }
}
