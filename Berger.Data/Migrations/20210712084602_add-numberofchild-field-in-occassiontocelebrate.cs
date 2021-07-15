using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addnumberofchildfieldinoccassiontocelebrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildDOB",
                table: "OccasionToCelebrate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirsChildDOB",
                table: "OccasionToCelebrate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondChildDOB",
                table: "OccasionToCelebrate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ThirdChildDOB",
                table: "OccasionToCelebrate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "ChequeBounceNotification",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirsChildDOB",
                table: "OccasionToCelebrate");

            migrationBuilder.DropColumn(
                name: "SecondChildDOB",
                table: "OccasionToCelebrate");

            migrationBuilder.DropColumn(
                name: "ThirdChildDOB",
                table: "OccasionToCelebrate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChildDOB",
                table: "OccasionToCelebrate",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "ChequeBounceNotification",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
