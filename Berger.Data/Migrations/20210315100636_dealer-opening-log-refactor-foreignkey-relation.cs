using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class dealeropeninglogrefactorforeignkeyrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpeningLogs_DealerInfos_DealerInfoId",
                table: "DealerOpeningLogs");

            migrationBuilder.DropIndex(
                name: "IX_DealerOpeningLogs_DealerInfoId",
                table: "DealerOpeningLogs");

            migrationBuilder.AddColumn<int>(
                name: "DealerOpeningId",
                table: "DealerOpeningLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_DealerOpeningId",
                table: "DealerOpeningLogs",
                column: "DealerOpeningId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs",
                column: "DealerOpeningId",
                principalTable: "DealerOpenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerOpeningLogs_DealerOpenings_DealerOpeningId",
                table: "DealerOpeningLogs");

            migrationBuilder.DropIndex(
                name: "IX_DealerOpeningLogs_DealerOpeningId",
                table: "DealerOpeningLogs");

            migrationBuilder.DropColumn(
                name: "DealerOpeningId",
                table: "DealerOpeningLogs");

            migrationBuilder.CreateIndex(
                name: "IX_DealerOpeningLogs_DealerInfoId",
                table: "DealerOpeningLogs",
                column: "DealerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerOpeningLogs_DealerInfos_DealerInfoId",
                table: "DealerOpeningLogs",
                column: "DealerInfoId",
                principalTable: "DealerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
