using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerInfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerClasification",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerGroup",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAP",
                table: "DealerInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PriceGroup",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOrg",
                table: "DealerInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "CustomerClasification",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "CustomerGroup",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "District",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsAP",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "PriceGroup",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "SalesOrg",
                table: "DealerInfos");
        }
    }
}
