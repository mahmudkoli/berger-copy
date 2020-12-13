using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class RenameDealerOpeingEntityProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineManagerId",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "SaleGroupCd",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "SaleOfficeCd",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "TerritoryNoCd",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "ZoneNoCd",
                table: "DealerOpenings");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeId",
                table: "DealerOpenings",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SaleGroup",
                table: "DealerOpenings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleOffice",
                table: "DealerOpenings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "DealerOpenings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "DealerOpenings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleGroup",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "SaleOffice",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "DealerOpenings");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "DealerOpenings");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeId",
                table: "DealerOpenings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineManagerId",
                table: "DealerOpenings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleGroupCd",
                table: "DealerOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleOfficeCd",
                table: "DealerOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryNoCd",
                table: "DealerOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneNoCd",
                table: "DealerOpenings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
