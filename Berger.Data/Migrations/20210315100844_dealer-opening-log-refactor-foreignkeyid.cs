using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class dealeropeninglogrefactorforeignkeyid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs");

            migrationBuilder.DropColumn(
                name: "DealerInfoId",
                table: "DealerOpeningLogs");

            migrationBuilder.AlterColumn<int>(
                name: "DealerOpeningId",
                table: "DealerOpeningLogs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs",
                column: "DealerOpeningId",
                principalTable: "DealerOpenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs");

            migrationBuilder.AlterColumn<int>(
                name: "DealerOpeningId",
                table: "DealerOpeningLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DealerInfoId",
                table: "DealerOpeningLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs",
                column: "DealerOpeningId",
                principalTable: "DealerOpenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
