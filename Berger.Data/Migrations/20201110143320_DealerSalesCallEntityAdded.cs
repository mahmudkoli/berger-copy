using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerSalesCallEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DealerSalesCalls",
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
                    Date = table.Column<DateTime>(nullable: false),
                    IsTargetPromotionCommunicated = table.Column<bool>(nullable: false),
                    IsTargetCommunicated = table.Column<bool>(nullable: false),
                    SecondarySalesRatings = table.Column<int>(nullable: false),
                    SecondarySalesReasonTitle = table.Column<string>(nullable: true),
                    SecondarySalesReasonRemarks = table.Column<string>(nullable: true),
                    IsOSCommunicated = table.Column<bool>(nullable: false),
                    IsSlippageCommunicated = table.Column<bool>(nullable: false),
                    IsPremiumProductCommunicated = table.Column<bool>(nullable: false),
                    IsPremiumProductLifting = table.Column<bool>(nullable: false),
                    PremiumProductLifting = table.Column<int>(nullable: true),
                    PremiumProductLiftingOthers = table.Column<string>(nullable: true),
                    IsCBInstalled = table.Column<bool>(nullable: false),
                    IsCBProductivityCommunicated = table.Column<bool>(nullable: false),
                    IsMerchendisingPlanogramFollowed = table.Column<bool>(nullable: false),
                    IsSubDealerInfluence = table.Column<bool>(nullable: false),
                    SubDealerInfluence = table.Column<int>(nullable: true),
                    IsPainterInfluence = table.Column<bool>(nullable: false),
                    PainterInfluence = table.Column<int>(nullable: true),
                    IsShopManProductKnowledgeDiscussed = table.Column<bool>(nullable: false),
                    IsShopManSalesTechniquesDiscussed = table.Column<bool>(nullable: false),
                    IsShopManMerchendizingImprovementDiscussed = table.Column<bool>(nullable: false),
                    IsCompetitionPresence = table.Column<bool>(nullable: false),
                    CompetitionPresence = table.Column<int>(nullable: true),
                    IsCompetitionBetterThanBPBL = table.Column<bool>(nullable: false),
                    CompetitionBetterThanBPBLRemarks = table.Column<string>(nullable: true),
                    CompetitionComments = table.Column<string>(nullable: true),
                    CompetitionImageId = table.Column<int>(nullable: true),
                    HasDealerSalesIssue = table.Column<bool>(nullable: false),
                    EnumDealerSalesIssue = table.Column<int>(nullable: true),
                    DealerSatisfaction = table.Column<int>(nullable: false),
                    DealerSatisfactionReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerSalesCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_Attachments_CompetitionImageId",
                        column: x => x.CompetitionImageId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerSalesCalls_DealerInfos_DealerId",
                        column: x => x.DealerId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerCompetitionSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerSalesCallId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    AverageMonthlySales = table.Column<decimal>(nullable: false),
                    ActualAMDSales = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerCompetitionSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerCompetitionSales_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerCompetitionSales_DealerSalesCalls_DealerSalesCallId",
                        column: x => x.DealerSalesCallId,
                        principalTable: "DealerSalesCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerSalesIssues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerSalesCallId = table.Column<int>(nullable: false),
                    MaterialName = table.Column<string>(nullable: true),
                    MaterialGroup = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    IsCBMachineMantainance = table.Column<bool>(nullable: false),
                    IsCBMachineMantainanceRegular = table.Column<bool>(nullable: false),
                    CBMachineMantainanceRegularReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerSalesIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerSalesIssues_DealerSalesCalls_DealerSalesCallId",
                        column: x => x.DealerSalesCallId,
                        principalTable: "DealerSalesCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerCompetitionSales_CompanyId",
                table: "DealerCompetitionSales",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerCompetitionSales_DealerSalesCallId",
                table: "DealerCompetitionSales",
                column: "DealerSalesCallId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionImageId",
                table: "DealerSalesCalls",
                column: "CompetitionImageId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_DealerId",
                table: "DealerSalesCalls",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_DealerSalesCallId",
                table: "DealerSalesIssues",
                column: "DealerSalesCallId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerCompetitionSales");

            migrationBuilder.DropTable(
                name: "DealerSalesIssues");

            migrationBuilder.DropTable(
                name: "DealerSalesCalls");
        }
    }
}
