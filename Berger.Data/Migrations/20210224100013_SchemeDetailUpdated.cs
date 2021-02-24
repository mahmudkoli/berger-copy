using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SchemeDetailUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "Item",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<string>(
                name: "BenefitDate",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateInDrum",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateInLtrOrKg",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchemeId",
                table: "SchemeDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenefitDate",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "RateInDrum",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "RateInLtrOrKg",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "SchemeDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
