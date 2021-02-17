using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class TintingUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TintingMachines_UserInfos_UserId",
                table: "TintingMachines");

            migrationBuilder.DropIndex(
                name: "IX_TintingMachines_UserId",
                table: "TintingMachines");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TintingMachines");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "TintingMachines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_UserInfoId",
                table: "TintingMachines",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TintingMachines_UserInfos_UserInfoId",
                table: "TintingMachines",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TintingMachines_UserInfos_UserInfoId",
                table: "TintingMachines");

            migrationBuilder.DropIndex(
                name: "IX_TintingMachines_UserInfoId",
                table: "TintingMachines");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "TintingMachines");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TintingMachines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TintingMachines_UserId",
                table: "TintingMachines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TintingMachines_UserInfos_UserId",
                table: "TintingMachines",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
