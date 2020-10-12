using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Berger.Data.Migrations
{
    public partial class SeedingDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DropdownTypes",
                columns: new[] { "CreatedBy", "CreatedTime", "Status", "WFStatus", "TypeName", "TypeCode" },
                values: new object[] { 1, DateTime.Now, 0, 0, "Dealer", "D01" }
                );
             migrationBuilder.InsertData(
                table: "DropdownTypes",
                columns: new[] { "CreatedBy", "CreatedTime", "Status", "WFStatus", "TypeName", "TypeCode" },
                values: new object[] { 1, DateTime.Now, 0, 0, "Employee", "E01" }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM DropdownTypes;");
        }
    }
}
