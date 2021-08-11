using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadBusinessProductrSourcingNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSourcingId",
                table: "LeadBusinessAchievements",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements",
                column: "ProductSourcingId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSourcingId",
                table: "LeadBusinessAchievements",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements",
                column: "ProductSourcingId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
