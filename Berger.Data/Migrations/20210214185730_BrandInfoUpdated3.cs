using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class BrandInfoUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatarialDescription",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "MatarialGroupOrBrand",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "MatrialCode",
                table: "BrandInfos");

            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "BrandInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialDescription",
                table: "BrandInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialGroupOrBrand",
                table: "BrandInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "MaterialDescription",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "MaterialGroupOrBrand",
                table: "BrandInfos");

            migrationBuilder.AddColumn<string>(
                name: "MatarialDescription",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatarialGroupOrBrand",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatrialCode",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
