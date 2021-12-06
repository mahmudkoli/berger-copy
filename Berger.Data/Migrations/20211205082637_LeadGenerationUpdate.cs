using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadGenerationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadGenerateFrom",
                table: "LeadGenerations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PainterCalls_PainterCatId",
                table: "PainterCalls",
                column: "PainterCatId");

            migrationBuilder.AddForeignKey(
                name: "FK_PainterCalls_DropdownDetails_PainterCatId",
                table: "PainterCalls",
                column: "PainterCatId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PainterCalls_DropdownDetails_PainterCatId",
                table: "PainterCalls");

            migrationBuilder.DropIndex(
                name: "IX_PainterCalls_PainterCatId",
                table: "PainterCalls");

            migrationBuilder.DropColumn(
                name: "LeadGenerateFrom",
                table: "LeadGenerations");
        }
    }
}
