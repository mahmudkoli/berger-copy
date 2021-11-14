using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SchemeNameinSchemeDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeDetails_SchemeMasters_SchemeMasterId",
                table: "SchemeDetails");

            migrationBuilder.DropIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "SchemeMasterId",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<string>(
                name: "BusinessArea",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchemeName",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchemeType",
                table: "SchemeDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessArea",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "SchemeName",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "SchemeType",
                table: "SchemeDetails");

            migrationBuilder.AddColumn<int>(
                name: "SchemeMasterId",
                table: "SchemeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SchemeDetails_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeDetails_SchemeMasters_SchemeMasterId",
                table: "SchemeDetails",
                column: "SchemeMasterId",
                principalTable: "SchemeMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
