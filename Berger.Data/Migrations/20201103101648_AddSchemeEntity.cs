using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class AddSchemeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "ZoneId",
            //    table: "UserZoneAreaMappings",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "TerritoryId",
            //    table: "UserZoneAreaMappings",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "SalesOfficeId",
            //    table: "UserZoneAreaMappings",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "AreaId",
            //    table: "UserZoneAreaMappings",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.CreateTable(
                name: "SchemeMasters",
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
                    SchemeName = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeMasters", x => x.Id);
                });




            migrationBuilder.CreateTable(
                name: "SchemeDetails",
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
                    Code = table.Column<string>(nullable: true),
                    Slab = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    TargetVolume= table.Column<string>(nullable: true),
                    Benefit = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    SchemeMasterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchemeDetails_SchemeMasters_SchemeMasterId",
                        column: x => x.SchemeMasterId,
                        principalTable: "SchemeMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "SchemeDetails");

            migrationBuilder.DropTable(
                name: "SchemeMasters");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ZoneId",
            //    table: "UserZoneAreaMappings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "TerritoryId",
            //    table: "UserZoneAreaMappings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "SalesOfficeId",
            //    table: "UserZoneAreaMappings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "AreaId",
            //    table: "UserZoneAreaMappings",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);
        }
    }
}
