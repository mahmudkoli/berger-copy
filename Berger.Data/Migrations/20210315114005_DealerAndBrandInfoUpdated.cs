using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerAndBrandInfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClubSupreme",
                table: "DealerInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLastYearAppointed",
                table: "DealerInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnamel",
                table: "BrandInfos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClubSupreme",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsLastYearAppointed",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsEnamel",
                table: "BrandInfos");
        }
    }
}
