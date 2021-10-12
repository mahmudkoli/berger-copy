using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadNOSBTypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "NumberOfStoriedBuilding",
                table: "LeadGenerations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "NumberOfStoriedBuilding",
                table: "LeadFollowUps",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfStoriedBuilding",
                table: "LeadGenerations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfStoriedBuilding",
                table: "LeadFollowUps",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
