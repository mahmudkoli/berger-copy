using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadFollowUPUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusPartialBusinessId",
                table: "LeadFollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusTotalLossId",
                table: "LeadFollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                table: "LeadFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_LeadFollowUps_ProjectStatusPartialBusinessId",
                table: "LeadFollowUps");

            migrationBuilder.DropIndex(
                name: "IX_LeadFollowUps_ProjectStatusTotalLossId",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ProjectStatusPartialBusinessId",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ProjectStatusTotalLossId",
                table: "LeadFollowUps");

            migrationBuilder.AlterColumn<int>(
                name: "SwappingCompetitionId",
                table: "LeadFollowUps",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "hasSwappingCompetition",
                table: "LeadFollowUps",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                table: "LeadFollowUps",
                column: "SwappingCompetitionId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "hasSwappingCompetition",
                table: "LeadFollowUps");

            migrationBuilder.AlterColumn<int>(
                name: "SwappingCompetitionId",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatusPartialBusinessId",
                table: "LeadFollowUps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatusTotalLossId",
                table: "LeadFollowUps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusPartialBusinessId",
                table: "LeadFollowUps",
                column: "ProjectStatusPartialBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFollowUps_ProjectStatusTotalLossId",
                table: "LeadFollowUps",
                column: "ProjectStatusTotalLossId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusPartialBusinessId",
                table: "LeadFollowUps",
                column: "ProjectStatusPartialBusinessId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_ProjectStatusTotalLossId",
                table: "LeadFollowUps",
                column: "ProjectStatusTotalLossId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFollowUps_DropdownDetails_SwappingCompetitionId",
                table: "LeadFollowUps",
                column: "SwappingCompetitionId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
