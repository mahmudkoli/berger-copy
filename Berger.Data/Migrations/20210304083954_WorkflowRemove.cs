using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class WorkflowRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserZoneAreaMappings");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserZoneAreaMappings");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserRoleMapping");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserRoleMapping");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserQuestionAnswerCollections");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserQuestionAnswerCollections");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "TintingMachines");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "TintingMachines");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SchemeMasters");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SchemeMasters");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "QuestionSets");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "QuestionSets");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "QuestionSetCollections");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "QuestionSetCollections");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "PainterCompanyMTDValues");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "PainterCompanyMTDValues");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "PainterAttachments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "PainterAttachments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuActivityPermissions");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuActivityPermissions");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "MenuActivities");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "MenuActivities");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "LeadGenerations");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "LeadGenerations");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "JourneyPlans");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "JourneyPlans");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "JourneyPlanMasters");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "JourneyPlanMasters");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "JourneyPlanDetails");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "JourneyPlanDetails");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "FocusDealers");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "FocusDealers");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "ELearningDocuments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "ELearningDocuments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "ELearningAttachments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "ELearningAttachments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DropdownTypes");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DropdownTypes");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DropdownDetails");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DropdownDetails");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DealerOpeningAttachments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DealerOpeningAttachments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "BrandInfos");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "WFStatus",
                table: "AttachedDealerPainters");

            migrationBuilder.DropColumn(
                name: "WorkflowId",
                table: "AttachedDealerPainters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserZoneAreaMappings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserZoneAreaMappings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserRoleMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserRoleMapping",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserQuestionAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserQuestionAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserQuestionAnswerCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserQuestionAnswerCollections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "UserInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "TintingMachines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "TintingMachines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SchemeMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SchemeMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "SchemeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "SchemeDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "QuestionSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "QuestionSets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "QuestionSetCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "QuestionSetCollections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "QuestionOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "QuestionOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Painters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Painters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "PainterCompanyMTDValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "PainterCompanyMTDValues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "PainterCalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "PainterCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "PainterAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "PainterAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuActivityPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuActivityPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "MenuActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "MenuActivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "LeadGenerations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "LeadGenerations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "LeadFollowUps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "JourneyPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "JourneyPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "JourneyPlanMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "JourneyPlanMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "JourneyPlanDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "JourneyPlanDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "FocusDealers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "FocusDealers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Examples",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Examples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "ELearningDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "ELearningDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "ELearningAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "ELearningAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DropdownTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DropdownTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DropdownDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DropdownDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DealerSalesCalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DealerOpenings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DealerOpenings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DealerOpeningAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DealerOpeningAttachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "DealerInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "DealerInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "BrandInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "BrandInfos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WFStatus",
                table: "AttachedDealerPainters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkflowId",
                table: "AttachedDealerPainters",
                type: "int",
                nullable: true);
        }
    }
}
