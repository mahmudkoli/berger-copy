using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addnewcolumninmenutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPermissions_Roles_RoleId",
                table: "MenuPermissions");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Menus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Menus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "MenuPermissions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmpRoleId",
                table: "MenuPermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MenuPermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPermissions_Roles_RoleId",
                table: "MenuPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuPermissions_Roles_RoleId",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "EmpRoleId",
                table: "MenuPermissions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MenuPermissions");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "MenuPermissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuPermissions_Roles_RoleId",
                table: "MenuPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
