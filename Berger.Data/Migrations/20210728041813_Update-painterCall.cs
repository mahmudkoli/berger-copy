using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UpdatepainterCall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PainterCode",
                table: "Painters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccChangeReason",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccDbblHolderName",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccDbblNumber",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAppInstalled",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Loyality",
                table: "PainterCalls",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NoOfPainterAttached",
                table: "PainterCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PainterName",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "PainterCalls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PainterCode",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "AccChangeReason",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "AccDbblHolderName",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "AccDbblNumber",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "IsAppInstalled",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Loyality",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "NoOfPainterAttached",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "PainterName",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "PainterCalls");
        }
    }
}
