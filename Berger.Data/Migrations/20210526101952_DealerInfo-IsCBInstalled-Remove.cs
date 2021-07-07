using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerInfoIsCBInstalledRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCBInstalled",
                table: "DealerInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCBInstalled",
                table: "DealerInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
