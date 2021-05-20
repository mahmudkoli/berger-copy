using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class CollectionPlanUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CollectionActualAmount",
                table: "CollectionPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SlippageAmount",
                table: "CollectionPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SlippageCollectionActualAmount",
                table: "CollectionPlans",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionActualAmount",
                table: "CollectionPlans");

            migrationBuilder.DropColumn(
                name: "SlippageAmount",
                table: "CollectionPlans");

            migrationBuilder.DropColumn(
                name: "SlippageCollectionActualAmount",
                table: "CollectionPlans");
        }
    }
}
