using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerInfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "DealerInfos");

            migrationBuilder.AddColumn<string>(
                name: "SalesGroup",
                table: "DealerInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesGroup",
                table: "DealerInfos");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "DealerInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "DealerInfos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
