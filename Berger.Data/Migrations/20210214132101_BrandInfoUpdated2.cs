using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class BrandInfoUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ersda",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "laeda",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "mtart",
                table: "BrandInfos");

            migrationBuilder.AddColumn<string>(
                name: "CreatedDate",
                table: "BrandInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "BrandInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedDate",
                table: "BrandInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BrandInfos");

            migrationBuilder.AddColumn<string>(
                name: "ersda",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "laeda",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mtart",
                table: "BrandInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
