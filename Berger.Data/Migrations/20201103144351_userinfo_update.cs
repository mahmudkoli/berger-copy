using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class userinfo_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserInfos",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserInfos");
        }
    }
}
