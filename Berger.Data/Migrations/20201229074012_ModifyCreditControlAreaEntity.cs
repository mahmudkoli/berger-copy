using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class ModifyCreditControlAreaEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_DropdownDetails_CreditControllAreaId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreditControllAreaId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreditControllAreaId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "CreditControlAreas");

            migrationBuilder.AddColumn<int>(
                name: "CreditControlAreaId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreditControlAreaId",
                table: "CreditControlAreas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreditControlAreas",
                table: "CreditControlAreas",
                column: "CreditControlAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditControlAreaId",
                table: "Payments",
                column: "CreditControlAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditControlAreas_CreditControlAreaId",
                table: "Payments",
                column: "CreditControlAreaId",
                principalTable: "CreditControlAreas",
                principalColumn: "CreditControlAreaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditControlAreas_CreditControlAreaId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreditControlAreaId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreditControlAreas",
                table: "CreditControlAreas");

            migrationBuilder.DropColumn(
                name: "CreditControlAreaId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreditControlAreaId",
                table: "CreditControlAreas");

            migrationBuilder.AddColumn<int>(
                name: "CreditControllAreaId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CreditControlAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditControllAreaId",
                table: "Payments",
                column: "CreditControllAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_DropdownDetails_CreditControllAreaId",
                table: "Payments",
                column: "CreditControllAreaId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
