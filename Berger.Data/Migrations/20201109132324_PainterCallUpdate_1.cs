using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PainterCallUpdate_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUsage",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "NewProBriefing",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "PremiumProtBriefing",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "SchemeComnunaction",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "UsageEftTools",
                table: "PainterCalls");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "PainterCalls",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAppUsage",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDbblIssue",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasNewProBriefing",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPremiumProtBriefing",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSchemeComnunaction",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasUsageEftTools",
                table: "PainterCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkInHandNumber",
                table: "PainterCalls",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasAppUsage",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasDbblIssue",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasNewProBriefing",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasPremiumProtBriefing",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasSchemeComnunaction",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "HasUsageEftTools",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "WorkInHandNumber",
                table: "PainterCalls");

            migrationBuilder.AddColumn<bool>(
                name: "AppUsage",
                table: "PainterCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NewProBriefing",
                table: "PainterCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "PainterCalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PremiumProtBriefing",
                table: "PainterCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SchemeComnunaction",
                table: "PainterCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsageEftTools",
                table: "PainterCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
