using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class JourneyPlanDetailsDealerInfoUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyPlanDetails_JourneyPlanMasters_JourneyPlanMasterId",
                table: "JourneyPlanDetails");

            migrationBuilder.DropIndex(
                name: "IX_JourneyPlanDetails_JourneyPlanMasterId",
                table: "JourneyPlanDetails");

            migrationBuilder.DropColumn(
                name: "JourneyPlanMasterId",
                table: "JourneyPlanDetails");

            migrationBuilder.DropColumn(
                name: "VisitDate",
                table: "JourneyPlanDetails");

            migrationBuilder.DropColumn(
                name: "IsAP",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "IsExclusive",
                table: "DealerInfos");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyPlanDetails_PlanId",
                table: "JourneyPlanDetails",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyPlanDetails_JourneyPlanMasters_PlanId",
                table: "JourneyPlanDetails",
                column: "PlanId",
                principalTable: "JourneyPlanMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JourneyPlanDetails_JourneyPlanMasters_PlanId",
                table: "JourneyPlanDetails");

            migrationBuilder.DropIndex(
                name: "IX_JourneyPlanDetails_PlanId",
                table: "JourneyPlanDetails");

            migrationBuilder.AddColumn<int>(
                name: "JourneyPlanMasterId",
                table: "JourneyPlanDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitDate",
                table: "JourneyPlanDetails",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAP",
                table: "DealerInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExclusive",
                table: "DealerInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyPlanDetails_JourneyPlanMasterId",
                table: "JourneyPlanDetails",
                column: "JourneyPlanMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_JourneyPlanDetails_JourneyPlanMasters_JourneyPlanMasterId",
                table: "JourneyPlanDetails",
                column: "JourneyPlanMasterId",
                principalTable: "JourneyPlanMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
