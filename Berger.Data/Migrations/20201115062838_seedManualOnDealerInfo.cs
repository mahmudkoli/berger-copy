using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class seedManualOnDealerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerZone",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "DealerInfos");

            migrationBuilder.AddColumn<string>(
                name: "CustZone",
                table: "DealerInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Division",
                table: "DealerInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DealerInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DealerInfos",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "CustZone",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DealerInfos");

            migrationBuilder.AddColumn<string>(
                name: "CustomerZone",
                table: "DealerInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "DealerInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
