using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class ModifyCreditControlAreaEntity_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentFrom",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "CustomerTypeId",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerTypeId",
                table: "Payments",
                column: "CustomerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_DropdownDetails_CustomerTypeId",
                table: "Payments",
                column: "CustomerTypeId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_DropdownDetails_CustomerTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CustomerTypeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CustomerTypeId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentFrom",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
