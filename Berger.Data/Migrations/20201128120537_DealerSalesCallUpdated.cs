using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerSalesCallUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_Attachments_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionProductDisplayImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<string>(
                name: "CompetitionProductDisplayImageUrl",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetitionSchemeModalityImageUrl",
                table: "DealerSalesCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DealerSalesCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_UserId",
                table: "DealerSalesCalls",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerSalesCalls_UserInfos_UserId",
                table: "DealerSalesCalls",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerSalesCalls_UserInfos_UserId",
                table: "DealerSalesCalls");

            migrationBuilder.DropIndex(
                name: "IX_DealerSalesCalls_UserId",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionProductDisplayImageUrl",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "CompetitionSchemeModalityImageUrl",
                table: "DealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DealerSalesCalls");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionProductDisplayImageId",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "DealerSalesCalls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionProductDisplayImageId",
                table: "DealerSalesCalls",
                column: "CompetitionProductDisplayImageId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerSalesCalls_CompetitionSchemeModalityImageId",
                table: "DealerSalesCalls",
                column: "CompetitionSchemeModalityImageId");

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
        }
    }
}
