using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UpdatePainterWithEmployeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

         

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Painters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PainterCalls",
                nullable: false,
                defaultValue: 0);


          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Painters");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PainterCalls");

        }
    }
}
