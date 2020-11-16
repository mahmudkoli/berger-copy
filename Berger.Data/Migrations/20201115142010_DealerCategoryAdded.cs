using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerCategoryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOffice",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "DealerInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "SalesOffice",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "DealerInfos");
        }
    }
}
