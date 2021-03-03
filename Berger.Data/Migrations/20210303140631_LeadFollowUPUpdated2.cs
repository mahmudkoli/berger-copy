using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadFollowUPUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hasSwappingCompetition",
                table: "LeadFollowUps",
                newName: "HasSwappingCompetition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasSwappingCompetition",
                table: "LeadFollowUps",
                newName: "hasSwappingCompetition");
        }
    }
}
