using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadFollowUpUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectStatusPartialBusinessPercentage",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldUnderCoatGallon",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldTopCoatGallon",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldInteriorKg",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldInteriorGallon",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldExteriorKg",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualVolumeSoldExteriorGallon",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaintJobCompletedInteriorPercentage",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaintJobCompletedExteriorPercentage",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Painters_PainterCatId",
                table: "Painters",
                column: "PainterCatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Painters_DropdownDetails_PainterCatId",
                table: "Painters",
                column: "PainterCatId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Painters_DropdownDetails_PainterCatId",
                table: "Painters");

            migrationBuilder.DropIndex(
                name: "IX_Painters_PainterCatId",
                table: "Painters");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectStatusPartialBusinessPercentage",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldUnderCoatGallon",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldTopCoatGallon",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldInteriorKg",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldInteriorGallon",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldExteriorKg",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualVolumeSoldExteriorGallon",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualPaintJobCompletedInteriorPercentage",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ActualPaintJobCompletedExteriorPercentage",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
