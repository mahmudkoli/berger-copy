using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class RemoveAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Painters_ParentId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ParentId",
                table: "Attachments");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Attachments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Attachments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
