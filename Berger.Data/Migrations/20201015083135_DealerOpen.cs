using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerOpen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "DealerOpeningId",
            //    table: "Attachments",
            //    nullable: true);

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
                    EmployeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerOpenings", x => x.Id);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Attachments_DealerOpeningId",
            //    table: "Attachments",
            //    column: "DealerOpeningId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Attachments_DealerOpenings_DealerOpeningId",
            //    table: "Attachments",
            //    column: "DealerOpeningId",
            //    principalTable: "DealerOpenings",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Attachments_DealerOpenings_DealerOpeningId",
            //    table: "Attachments");

            migrationBuilder.DropTable(
                name: "DealerOpenings");

            //migrationBuilder.DropIndex(
            //    name: "IX_Attachments_DealerOpeningId",
            //    table: "Attachments");

            //migrationBuilder.DropColumn(
            //    name: "DealerOpeningId",
            //    table: "Attachments");
        }
    }
}
