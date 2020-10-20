﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PainterCall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PainterCalls",
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
                    SchemeComnunaction = table.Column<bool>(nullable: false),
                    PremiumProtBriefing = table.Column<bool>(nullable: false),
                    NewProBriefing = table.Column<bool>(nullable: false),
                    UsageEftTools = table.Column<bool>(nullable: false),
                    AppUsage = table.Column<bool>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    PainterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainterCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PainterCalls_Painters_PainterId",
                        column: x => x.PainterId,
                        principalTable: "Painters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PainterCompanyMTDValues",
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
                    CompanyId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    CountInPercent = table.Column<float>(nullable: false),
                    PainterCallId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PainterCompanyMTDValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PainterCompanyMTDValues_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PainterCompanyMTDValues_PainterCalls_PainterCallId",
                        column: x => x.PainterCallId,
                        principalTable: "PainterCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PainterCalls_PainterId",
                table: "PainterCalls",
                column: "PainterId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterCompanyMTDValues_CompanyId",
                table: "PainterCompanyMTDValues",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PainterCompanyMTDValues_PainterCallId",
                table: "PainterCompanyMTDValues",
                column: "PainterCallId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PainterCompanyMTDValues");

            migrationBuilder.DropTable(
                name: "PainterCalls");
        }
    }
}