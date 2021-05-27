using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class DealerInfoIsClubsupremeRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClubSupreme",
                table: "DealerInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClubSupreme",
                table: "DealerInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
