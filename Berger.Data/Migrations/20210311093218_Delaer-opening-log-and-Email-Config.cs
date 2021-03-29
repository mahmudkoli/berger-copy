using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DelaeropeninglogandEmailConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentApprovarId",
                table: "DealerOpenings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextApprovarId",
                table: "DealerOpenings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealerOpeningLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DealerInfoId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpeningLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerOpeningLogs_DealerInfos_DealerInfoId",
                        column: x => x.DealerInfoId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerOpeningLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfigForDealerOppenings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Designation = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigForDealerOppenings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpenings_CurrentApprovarId",
                table: "DealerOpenings",
                column: "CurrentApprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpenings_NextApprovarId",
                table: "DealerOpenings",
                column: "NextApprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_DealerInfoId",
                table: "DealerOpeningLogs",
                column: "DealerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_UserId",
                table: "DealerOpeningLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpenings_UserInfos_CurrentApprovarId",
                table: "DealerOpenings",
                column: "CurrentApprovarId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpenings_UserInfos_NextApprovarId",
                table: "DealerOpenings",
                column: "NextApprovarId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpenings_UserInfos_CurrentApprovarId",
                table: "DealerOpenings");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpenings_UserInfos_NextApprovarId",
                table: "DealerOpenings");

            migrationBuilder.DropTable(
                name: "DealerOpeningLogs");

            migrationBuilder.DropTable(
                name: "EmailConfigForDealerOppenings");

            migrationBuilder.DropIndex(
                name: "IX_DealerOpenings_CurrentApprovarId",
                table: "DealerOpenings");

            migrationBuilder.DropIndex(
                name: "IX_DealerOpenings_NextApprovarId",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "CurrentApprovarId",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "NextApprovarId",
                table: "DealerOpenings");
        }
    }
}
