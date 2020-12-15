using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadBusinessAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BergerValueSales = table.Column<decimal>(nullable: false),
                    BergerPremiumBrandSalesValue = table.Column<decimal>(nullable: false),
                    CompetitionValueSales = table.Column<decimal>(nullable: false),
                    ProductSourcing = table.Column<string>(nullable: true),
                    IsColorSchemeGiven = table.Column<bool>(nullable: false),
                    IsProductSampling = table.Column<bool>(nullable: false),
                    ProductSamplingBrandName = table.Column<string>(nullable: true),
                    NextVisitDate = table.Column<DateTime>(nullable: false),
                    RemarksOrOutcome = table.Column<string>(nullable: true),
                    PhotoCaptureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadBusinessAchievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadGenerations",
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
                    UserId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Depot = table.Column<string>(nullable: true),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    TypeOfClientId = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    ProjectAddress = table.Column<string>(nullable: true),
                    KeyContactPersonName = table.Column<string>(nullable: true),
                    KeyContactPersonMobile = table.Column<string>(nullable: true),
                    PaintContractorName = table.Column<string>(nullable: true),
                    PaintContractorMobile = table.Column<string>(nullable: true),
                    PaintingStageId = table.Column<int>(nullable: false),
                    VisitDate = table.Column<DateTime>(nullable: false),
                    ExpectedDateOfPainting = table.Column<DateTime>(nullable: false),
                    NumberOfStoriedBuilding = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInteriorChangeCount = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExteriorChangeCount = table.Column<int>(nullable: false),
                    ExpectedValue = table.Column<decimal>(nullable: false),
                    ExpectedValueChangeCount = table.Column<int>(nullable: false),
                    ExpectedMonthlyBusinessValue = table.Column<decimal>(nullable: false),
                    ExpectedMonthlyBusinessValueChangeCount = table.Column<int>(nullable: false),
                    RequirementOfColorScheme = table.Column<bool>(nullable: false),
                    ProductSamplingRequired = table.Column<bool>(nullable: false),
                    NextFollowUpDate = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    PhotoCaptureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadGenerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_DropdownDetails_PaintingStageId",
                        column: x => x.PaintingStageId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_DropdownDetails_TypeOfClientId",
                        column: x => x.TypeOfClientId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadGenerations_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadFollowUps",
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
                    LeadGenerationId = table.Column<int>(nullable: false),
                    LastVisitedDate = table.Column<DateTime>(nullable: false),
                    NextVisitDatePlan = table.Column<DateTime>(nullable: false),
                    ActualVisitDate = table.Column<DateTime>(nullable: false),
                    TypeOfClientId = table.Column<int>(nullable: false),
                    KeyContactPersonName = table.Column<string>(nullable: true),
                    KeyContactPersonNameChangeReason = table.Column<string>(nullable: true),
                    KeyContactPersonMobile = table.Column<string>(nullable: true),
                    KeyContactPersonMobileChangeReason = table.Column<string>(nullable: true),
                    PaintContractorName = table.Column<string>(nullable: true),
                    PaintContractorNameChangeReason = table.Column<string>(nullable: true),
                    PaintContractorMobile = table.Column<string>(nullable: true),
                    PaintContractorMobileChangeReason = table.Column<string>(nullable: true),
                    NumberOfStoriedBuilding = table.Column<int>(nullable: false),
                    NumberOfStoriedBuildingChangeReason = table.Column<string>(nullable: true),
                    ExpectedValue = table.Column<decimal>(nullable: false),
                    ExpectedValueChangeReason = table.Column<string>(nullable: true),
                    ExpectedMonthlyBusinessValue = table.Column<decimal>(nullable: false),
                    ExpectedMonthlyBusinessValueChangeReason = table.Column<string>(nullable: true),
                    ProjectStatusId = table.Column<int>(nullable: false),
                    ProjectStatusLeadCompletedId = table.Column<int>(nullable: true),
                    ProjectStatusTotalLossId = table.Column<int>(nullable: true),
                    ProjectStatusTotalLossRemarks = table.Column<string>(nullable: true),
                    ProjectStatusPartialBusinessId = table.Column<int>(nullable: true),
                    ProjectStatusPartialBusinessPercentage = table.Column<int>(nullable: false),
                    SwappingCompetitionId = table.Column<int>(nullable: false),
                    SwappingCompetitionAnotherCompetitorName = table.Column<string>(nullable: true),
                    TotalPaintingAreaSqftInterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftInteriorChangeReason = table.Column<string>(nullable: true),
                    TotalPaintingAreaSqftExterior = table.Column<int>(nullable: false),
                    TotalPaintingAreaSqftExteriorChangeReason = table.Column<string>(nullable: true),
                    UpTradingFromBrandName = table.Column<string>(nullable: true),
                    UpTradingToBrandName = table.Column<string>(nullable: true),
                    BrandUsedInteriorBrandName = table.Column<string>(nullable: true),
                    BrandUsedExteriorBrandName = table.Column<string>(nullable: true),
                    BrandUsedUnderCoatBrandName = table.Column<string>(nullable: true),
                    BrandUsedTopCoatBrandName = table.Column<string>(nullable: true),
                    ActualPaintJobCompletedInteriorPercentage = table.Column<int>(nullable: false),
                    ActualPaintJobCompletedExteriorPercentage = table.Column<int>(nullable: false),
                    ActualVolumeSoldInteriorGallon = table.Column<int>(nullable: false),
                    ActualVolumeSoldInteriorKg = table.Column<int>(nullable: false),
                    ActualVolumeSoldExteriorGallon = table.Column<int>(nullable: false),
                    ActualVolumeSoldExteriorKg = table.Column<int>(nullable: false),
                    ActualVolumeSoldUnderCoatGallon = table.Column<int>(nullable: false),
                    ActualVolumeSoldTopCoatGallon = table.Column<int>(nullable: false),
                    BusinessAchievementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_LeadBusinessAchievements_BusinessAchievementId",
                        column: x => x.BusinessAchievementId,
                        principalTable: "LeadBusinessAchievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_LeadGenerations_LeadGenerationId",
                        column: x => x.LeadGenerationId,
                        principalTable: "LeadGenerations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusId",
                        column: x => x.ProjectStatusId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusLeadCompletedId",
                        column: x => x.ProjectStatusLeadCompletedId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusPartialBusinessId",
                        column: x => x.ProjectStatusPartialBusinessId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusTotalLossId",
                        column: x => x.ProjectStatusTotalLossId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                        column: x => x.SwappingCompetitionId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeadFollowUps_DropdownDetails_TypeOfClientId",
                        column: x => x.TypeOfClientId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_BusinessAchievementId",
                table: "LeadFollowUps",
                column: "BusinessAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_LeadGenerationId",
                table: "LeadFollowUps",
                column: "LeadGenerationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusId",
                table: "LeadFollowUps",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusLeadCompletedId",
                table: "LeadFollowUps",
                column: "ProjectStatusLeadCompletedId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusPartialBusinessId",
                table: "LeadFollowUps",
                column: "ProjectStatusPartialBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusTotalLossId",
                table: "LeadFollowUps",
                column: "ProjectStatusTotalLossId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_SwappingCompetitionId",
                table: "LeadFollowUps",
                column: "SwappingCompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_TypeOfClientId",
                table: "LeadFollowUps",
                column: "TypeOfClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_PaintingStageId",
                table: "LeadGenerations",
                column: "PaintingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_TypeOfClientId",
                table: "LeadGenerations",
                column: "TypeOfClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadGenerations_UserId",
                table: "LeadGenerations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadFollowUps");

            migrationBuilder.DropTable(
                name: "LeadBusinessAchievements");

            migrationBuilder.DropTable(
                name: "LeadGenerations");
        }
    }
}
