using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class FixMigrationAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Painters_PainterId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PainterId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PainterId",
                table: "Attachments");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ParentId",
                table: "Attachments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments",
                column: "ParentId",
                principalTable: "Painters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ParentId",
                table: "Attachments");

            migrationBuilder.AddColumn<int>(
                name: "PainterId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PainterId",
                table: "Attachments",
                column: "PainterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Painters_PainterId",
                table: "Attachments",
                column: "PainterId",
                principalTable: "Painters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
