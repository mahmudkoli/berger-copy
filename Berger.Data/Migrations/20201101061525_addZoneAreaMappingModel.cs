using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addZoneAreaMappingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "HomePhone",
            //    table: "UserInfos");



            migrationBuilder.CreateTable(
                name: "UserZoneAreaMappings",
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
                    PlanId = table.Column<int>(nullable: false),
                    SalesOfficeId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    TerritoryId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserZoneAreaMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserZoneAreaMappings_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_UserZoneAreaMappings_UserInfoId",
                table: "UserZoneAreaMappings",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "UserZoneAreaMappings");

            

          


            //migrationBuilder.AddColumn<string>(
            //    name: "HomePhone",
            //    table: "UserInfos",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }
    }
}
