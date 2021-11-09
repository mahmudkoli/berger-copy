using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class MobileAppLogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileAppLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    BearerToken = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    LastActivity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAppLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileAppLogs");
        }
    }
}
