using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addedTintiningMachineEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TintiningMachines",
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
                    TerritoryCd = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    No = table.Column<decimal>(nullable: false),
                    Cont = table.Column<float>(nullable: false),
                    NoOfCorrection = table.Column<int>(nullable: true),
                    FormCorrectionMode = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TintiningMachines");
        }
    }
}
