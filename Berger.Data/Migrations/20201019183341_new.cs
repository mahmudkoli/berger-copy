using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Attachments",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        CreatedTime = table.Column<DateTime>(nullable: false),
            //        ModifiedBy = table.Column<int>(nullable: true),
            //        ModifiedTime = table.Column<DateTime>(nullable: true),
            //        Status = table.Column<int>(nullable: false),
            //        WorkflowId = table.Column<int>(nullable: true),
            //        WFStatus = table.Column<int>(nullable: false),
            //        Name = table.Column<string>(nullable: true),
            //        Path = table.Column<string>(nullable: true),
            //        Size = table.Column<long>(nullable: false),
            //        Format = table.Column<string>(nullable: true),
            //        TableName = table.Column<string>(nullable: true),
            //        ParentId = table.Column<int>(nullable: false),
            //        PainterId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Attachments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Attachments_Painters_PainterId",
            //            column: x => x.PainterId,
            //            principalTable: "Painters",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Attachments_PainterId",
            //    table: "Attachments",
            //    column: "PainterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Attachments");
        }
    }
}
