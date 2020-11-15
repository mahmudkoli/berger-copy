using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class AddingJourneyMasterDetailEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JourneyPlanMasters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    LineManagerId = table.Column<int>(nullable: true),
                    PlanDate = table.Column<DateTime>(nullable: false),
                    ApprovedById = table.Column<int>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RejectedBy = table.Column<int>(nullable: false),
                    RejectedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyPlanMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JourneyPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    VisitDate = table.Column<DateTime>(nullable: false),
                    PlanId = table.Column<int>(nullable: false),
                    JourneyPlanMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyPlanDetails_JourneyPlanMasters_JourneyPlanMasterId",
                        column: x => x.JourneyPlanMasterId,
                        principalTable: "JourneyPlanMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JourneyPlanDetails_JourneyPlanMasterId",
                table: "JourneyPlanDetails",
                column: "JourneyPlanMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JourneyPlanDetails");

            migrationBuilder.DropTable(
                name: "JourneyPlanMasters");
        }
    }
}
