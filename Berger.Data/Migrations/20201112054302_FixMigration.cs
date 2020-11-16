using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class FixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AttachedDealerCd",
                table: "Painters",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LineManagerId",
                table: "JourneyPlanMasters",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealerOpenings",
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
                    BusinessArea = table.Column<string>(nullable: true),
                    SaleOfficeCd = table.Column<string>(nullable: true),
                    SaleGroupCd = table.Column<string>(nullable: true),
                    TerritoryNoCd = table.Column<string>(nullable: true),
                    ZoneNoCd = table.Column<string>(nullable: true),
                    EmployeId = table.Column<int>(nullable: false),
                    LineManagerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpenings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "LineManagerId",
                table: "JourneyPlanMasters");

            migrationBuilder.AlterColumn<int>(
                name: "AttachedDealerCd",
                table: "Painters",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
