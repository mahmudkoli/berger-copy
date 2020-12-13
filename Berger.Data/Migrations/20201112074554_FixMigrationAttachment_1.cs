using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class FixMigrationAttachment_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Attachments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PainterId",
                table: "Attachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments",
                column: "ParentId",
                principalTable: "Painters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PainterId",
                table: "Attachments");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Attachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments",
                column: "ParentId",
                principalTable: "Painters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
