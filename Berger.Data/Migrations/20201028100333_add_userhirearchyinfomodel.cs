using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class add_userhirearchyinfomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartMent",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePhone",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginName",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginNameWithDomain",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserHirearchyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Plan = table.Column<string>(nullable: true),
                    SaleOffice = table.Column<int>(nullable: false),
                    AreaGroup = table.Column<int>(nullable: false),
                    Territory = table.Column<string>(nullable: true),
                    Zone = table.Column<int>(nullable: false),
                    UserInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHirearchyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHirearchyInfos_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHirearchyInfos_UserInfoId",
                table: "UserHirearchyInfos",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHirearchyInfos");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "DepartMent",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "HomePhone",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "LoginName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "LoginNameWithDomain",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "UserInfos");
        }
    }
}
