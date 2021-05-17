using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherClientName",
                table: "LeadGenerations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherClientName",
                table: "LeadFollowUps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherClientName",
                table: "LeadGenerations");

            migrationBuilder.DropColumn(
                name: "OtherClientName",
                table: "LeadFollowUps");
        }
    }
}
