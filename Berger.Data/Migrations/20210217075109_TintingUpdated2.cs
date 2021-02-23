using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class TintingUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TintiningMachines");

            migrationBuilder.CreateTable(
                name: "TintingMachines",
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
                    Territory = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    NoOfActiveMachine = table.Column<int>(nullable: false),
                    NoOfInactiveMachine = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Contribution = table.Column<decimal>(nullable: false),
                    NoOfCorrection = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TintingMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TintingMachines_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TintingMachines_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_CompanyId",
                table: "TintingMachines",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_UserId",
                table: "TintingMachines",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TintingMachines");

            migrationBuilder.CreateTable(
                name: "TintiningMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Cont = table.Column<float>(type: "real", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    No = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfCorrection = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TerritoryCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WFStatus = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TintiningMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TintiningMachines_DropdownDetails_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TintiningMachines_CompanyId",
                table: "TintiningMachines",
                column: "CompanyId");
        }
    }
}
