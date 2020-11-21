using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealserSalesCallEntityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_CompetitionImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsCBMachineMantainance",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "IsCBMachineMantainanceRegular",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "CompetitionBetterThanBPBLRemarks",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionComments",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionPresence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "DealerSatisfaction",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "EnumDealerSalesIssue",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsCompetitionBetterThanBPBL",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsCompetitionPresence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsMerchendisingPlanogramFollowed",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsPainterInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsSubDealerInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "PainterInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "PremiumProductLifting",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "SecondarySalesRatings",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "SubDealerInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<int>(
                name: "CBMachineMantainanceId",
                table: "DealerSalesIssues",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealerSalesIssueCategoryId",
                table: "DealerSalesIssues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCBMachineMantainance",
                table: "DealerSalesIssues",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "DealerSalesIssues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BPBLActualAMDSales",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BPBLAverageMonthlySales",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionProductDisplayBetterThanBPBLRemarks",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionProductDisplayImageId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionSchemeModalityComments",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionServiceBetterThanBPBLRemarks",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionShopBoysComments",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealerSatisfactionId",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBPBLSales",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCompetitionPresence",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasOS",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPainterInfluence",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSlippage",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSubDealerInfluence",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompetitionProductDisplayBetterThanBPBL",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompetitionServiceBetterThanBPBL",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubDealerCall",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MerchendisingId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PainterInfluenceId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PremiumProductLiftingId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondarySalesRatingsId",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubDealerInfluenceId",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_CBMachineMantainanceId",
                table: "DealerSalesIssues",
                column: "CBMachineMantainanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_DealerSalesIssueCategoryId",
                table: "DealerSalesIssues",
                column: "DealerSalesIssueCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesIssues_PriorityId",
                table: "DealerSalesIssues",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls",
                column: "CompetitionProductDisplayImageId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls",
                column: "CompetitionSchemeModalityImageId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_DealerSatisfactionId",
                table: "DealerSalesCalls",
                column: "DealerSatisfactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_MerchendisingId",
                table: "DealerSalesCalls",
                column: "MerchendisingId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_PainterInfluenceId",
                table: "DealerSalesCalls",
                column: "PainterInfluenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_PremiumProductLiftingId",
                table: "DealerSalesCalls",
                column: "PremiumProductLiftingId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_SecondarySalesRatingsId",
                table: "DealerSalesCalls",
                column: "SecondarySalesRatingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_SubDealerInfluenceId",
                table: "DealerSalesCalls",
                column: "SubDealerInfluenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls",
                column: "CompetitionProductDisplayImageId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls",
                column: "CompetitionSchemeModalityImageId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_DealerSatisfactionId",
                table: "DealerSalesCalls",
                column: "DealerSatisfactionId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_MerchendisingId",
                table: "DealerSalesCalls",
                column: "MerchendisingId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_PainterInfluenceId",
                table: "DealerSalesCalls",
                column: "PainterInfluenceId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_PremiumProductLiftingId",
                table: "DealerSalesCalls",
                column: "PremiumProductLiftingId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_SecondarySalesRatingsId",
                table: "DealerSalesCalls",
                column: "SecondarySalesRatingsId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_SubDealerInfluenceId",
                table: "DealerSalesCalls",
                column: "SubDealerInfluenceId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_CBMachineMantainanceId",
                table: "DealerSalesIssues",
                column: "CBMachineMantainanceId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_DealerSalesIssueCategoryId",
                table: "DealerSalesIssues",
                column: "DealerSalesIssueCategoryId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_PriorityId",
                table: "DealerSalesIssues",
                column: "PriorityId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_DealerSatisfactionId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_MerchendisingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_PainterInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_PremiumProductLiftingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_SecondarySalesRatingsId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_DropdownDetails_SubDealerInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_CBMachineMantainanceId",
                table: "DealerSalesIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_DealerSalesIssueCategoryId",
                table: "DealerSalesIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesIssues_DropdownDetails_PriorityId",
                table: "DealerSalesIssues");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesIssues_CBMachineMantainanceId",
                table: "DealerSalesIssues");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesIssues_DealerSalesIssueCategoryId",
                table: "DealerSalesIssues");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesIssues_PriorityId",
                table: "DealerSalesIssues");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_DealerSatisfactionId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_MerchendisingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_PainterInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_PremiumProductLiftingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_SecondarySalesRatingsId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_SubDealerInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CBMachineMantainanceId",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "DealerSalesIssueCategoryId",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "HasCBMachineMantainance",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "DealerSalesIssues");

            migrationBuilder.DropColumn(
                name: "BPBLActualAMDSales",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "BPBLAverageMonthlySales",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionProductDisplayBetterThanBPBLRemarks",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionSchemeModalityComments",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionServiceBetterThanBPBLRemarks",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionShopBoysComments",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "DealerSatisfactionId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasBPBLSales",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasCompetitionPresence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasOS",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasPainterInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasSlippage",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "HasSubDealerInfluence",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsCompetitionProductDisplayBetterThanBPBL",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsCompetitionServiceBetterThanBPBL",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "IsSubDealerCall",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "MerchendisingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "PainterInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "PremiumProductLiftingId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "SecondarySalesRatingsId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "SubDealerInfluenceId",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<bool>(
                name: "IsCBMachineMantainance",
                table: "DealerSalesIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCBMachineMantainanceRegular",
                table: "DealerSalesIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "DealerSalesIssues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionBetterThanBPBLRemarks",
                table: "DealerSalesCalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionComments",
                table: "DealerSalesCalls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionImageId",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionPresence",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealerSatisfaction",
                table: "DealerSalesCalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnumDealerSalesIssue",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompetitionBetterThanBPBL",
                table: "DealerSalesCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompetitionPresence",
                table: "DealerSalesCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMerchendisingPlanogramFollowed",
                table: "DealerSalesCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPainterInfluence",
                table: "DealerSalesCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubDealerInfluence",
                table: "DealerSalesCalls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PainterInfluence",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PremiumProductLifting",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondarySalesRatings",
                table: "DealerSalesCalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubDealerInfluence",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionImageId",
                table: "DealerSalesCalls",
                column: "CompetitionImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionImageId",
                table: "DealerSalesCalls",
                column: "CompetitionImageId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
