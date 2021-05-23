using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class SchemeDetailBenefitStartDateBenefitEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BenefitEndDate",
                table: "SchemeDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BenefitStartDate",
                table: "SchemeDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenefitEndDate",
                table: "SchemeDetails");

            migrationBuilder.DropColumn(
                name: "BenefitStartDate",
                table: "SchemeDetails");
        }
    }
}
