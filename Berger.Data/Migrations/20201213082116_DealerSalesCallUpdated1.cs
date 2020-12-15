using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerSalesCallUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JourneyPlanId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_JourneyPlanId",
                table: "DealerSalesCalls",
                column: "JourneyPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_JourneyPlanMasters_JourneyPlanId",
                table: "DealerSalesCalls",
                column: "JourneyPlanId",
                principalTable: "JourneyPlanMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_JourneyPlanMasters_JourneyPlanId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_JourneyPlanId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "JourneyPlanId",
                table: "DealerSalesCalls");
        }
    }
}
