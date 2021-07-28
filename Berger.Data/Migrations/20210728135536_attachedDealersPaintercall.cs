using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class attachedDealersPaintercall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachedDealerPainterCalls_DealerInfos_DealerIdId",
                table: "AttachedDealerPainterCalls");

            migrationBuilder.DropIndex(
                name: "IX_AttachedDealerPainterCalls_DealerIdId",
                table: "AttachedDealerPainterCalls");

            migrationBuilder.DropColumn(
                name: "DealerIdId",
                table: "AttachedDealerPainterCalls");

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDealerPainterCalls_DealerId",
                table: "AttachedDealerPainterCalls",
                column: "DealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachedDealerPainterCalls_DealerInfos_DealerId",
                table: "AttachedDealerPainterCalls",
                column: "DealerId",
                principalTable: "DealerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachedDealerPainterCalls_DealerInfos_DealerId",
                table: "AttachedDealerPainterCalls");

            migrationBuilder.DropIndex(
                name: "IX_AttachedDealerPainterCalls_DealerId",
                table: "AttachedDealerPainterCalls");

            migrationBuilder.AddColumn<int>(
                name: "DealerIdId",
                table: "AttachedDealerPainterCalls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDealerPainterCalls_DealerIdId",
                table: "AttachedDealerPainterCalls",
                column: "DealerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachedDealerPainterCalls_DealerInfos_DealerIdId",
                table: "AttachedDealerPainterCalls",
                column: "DealerIdId",
                principalTable: "DealerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
