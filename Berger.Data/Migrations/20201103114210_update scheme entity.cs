using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class updateschemeentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeBenefits_SchemeMasters_SchemeMasterId",
                table: "SchemeBenefits");

            migrationBuilder.DropIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails");

            migrationBuilder.DropIndex(
                name: "IX_SchemeBenefits_SchemeMasterId",
                table: "SchemeBenefits");

            migrationBuilder.DropColumn(
                name: "SchemeMasterId",
                table: "SchemeBenefits");

            migrationBuilder.AddColumn<int>(
                name: "SchemeDeatailId",
                table: "SchemeBenefits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SchemeBenefits_SchemeDeatailId",
                table: "SchemeBenefits",
                column: "SchemeDeatailId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeBenefits_SchemeDetails_SchemeDeatailId",
                table: "SchemeBenefits",
                column: "SchemeDeatailId",
                principalTable: "SchemeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeBenefits_SchemeDetails_SchemeDeatailId",
                table: "SchemeBenefits");

            migrationBuilder.DropIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails");

            migrationBuilder.DropIndex(
                name: "IX_SchemeBenefits_SchemeDeatailId",
                table: "SchemeBenefits");

            migrationBuilder.DropColumn(
                name: "SchemeDeatailId",
                table: "SchemeBenefits");

            migrationBuilder.AddColumn<int>(
                name: "SchemeMasterId",
                table: "SchemeBenefits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchemeBenefits_SchemeMasterId",
                table: "SchemeBenefits",
                column: "SchemeMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeBenefits_SchemeMasters_SchemeMasterId",
                table: "SchemeBenefits",
                column: "SchemeMasterId",
                principalTable: "SchemeMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
