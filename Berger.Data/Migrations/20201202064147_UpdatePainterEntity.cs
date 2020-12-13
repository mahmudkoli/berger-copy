using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UpdatePainterEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepotName",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "SaleGroupCd",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "TerritroyCd",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "ZoneCd",
                table: "Painters");

            migrationBuilder.AddColumn<string>(
                name: "Depot",
                table: "Painters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleGroup",
                table: "Painters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "Painters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Painters",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttachedDealerPainters",
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
                    Dealer = table.Column<int>(nullable: false),
                    PainterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedDealerPainters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachedDealerPainters_Painters_PainterId",
                        column: x => x.PainterId,
                        principalTable: "Painters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDealerPainters_PainterId",
                table: "AttachedDealerPainters",
                column: "PainterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedDealerPainters");

            migrationBuilder.DropColumn(
                name: "Depot",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "SaleGroup",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Painters");

            migrationBuilder.AddColumn<string>(
                name: "DepotName",
                table: "Painters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleGroupCd",
                table: "Painters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritroyCd",
                table: "Painters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneCd",
                table: "Painters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
