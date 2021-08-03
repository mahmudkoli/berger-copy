using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class attachedDealerPaintercall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealerIdId",
                table: "AttachedDealerPainterCalls",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
