using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class MerchandisingSnapShotUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OthersSnapShotCategoryName",
                table: "MerchandisingSnapShots",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OthersSnapShotCategoryName",
                table: "MerchandisingSnapShots");
        }
    }
}
