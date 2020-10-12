using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class PainterEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Painters",
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
                    DepotName = table.Column<string>(nullable: true),
                    SaleGroup = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    HasDbbl = table.Column<bool>(nullable: false),
                    AccNumber = table.Column<string>(nullable: true),
                    AccHolderName = table.Column<string>(nullable: true),
                    PersonlIdentityNo = table.Column<string>(nullable: true),
                    PainterImage = table.Column<string>(nullable: true),
                    IsAppInstalled = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    AvgMonthlyVal = table.Column<string>(nullable: true),
                    Loality = table.Column<float>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    PainterCatId = table.Column<int>(nullable: false),
                    TerritoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Painters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Painters_DropdownDetails_DealerId",
                        column: x => x.DealerId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Painters_DropdownDetails_PainterCatId",
                        column: x => x.PainterCatId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Painters_DropdownDetails_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Painters_DealerId",
                table: "Painters",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Painters_PainterCatId",
                table: "Painters",
                column: "PainterCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Painters_TerritoryId",
                table: "Painters",
                column: "TerritoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Painters");
        }
    }
}
