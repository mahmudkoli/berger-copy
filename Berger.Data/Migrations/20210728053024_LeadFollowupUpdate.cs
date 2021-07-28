using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class LeadFollowupUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldExteriorGallon",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldExteriorKg",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldInteriorGallon",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldInteriorKg",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldTopCoatGallon",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ActualVolumeSoldUnderCoatGallon",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ProductSourcing",
                table: "LeadBusinessAchievements");

            migrationBuilder.AddColumn<string>(
                name: "ProjectStatusHandOverRemarks",
                table: "LeadFollowUps",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSourcingId",
                table: "LeadBusinessAchievements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductSourcingRemarks",
                table: "LeadBusinessAchievements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeadActualVolumeSold",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeadFollowUpId = table.Column<int>(nullable: false),
                    BrandInfoId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    ActualVolumeSoldType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadActualVolumeSold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadActualVolumeSold_BrandInfos_BrandInfoId",
                        column: x => x.BrandInfoId,
                        principalTable: "BrandInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadActualVolumeSold_LeadFollowUps_LeadFollowUpId",
                        column: x => x.LeadFollowUpId,
                        principalTable: "LeadFollowUps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadBusinessAchievements_ProductSourcingId",
                table: "LeadBusinessAchievements",
                column: "ProductSourcingId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadActualVolumeSold_BrandInfoId",
                table: "LeadActualVolumeSold",
                column: "BrandInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadActualVolumeSold_LeadFollowUpId",
                table: "LeadActualVolumeSold",
                column: "LeadFollowUpId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements",
                column: "ProductSourcingId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadBusinessAchievements_DropdownDetails_ProductSourcingId",
                table: "LeadBusinessAchievements");

            migrationBuilder.DropTable(
                name: "LeadActualVolumeSold");

            migrationBuilder.DropIndex(
                name: "IX_LeadBusinessAchievements_ProductSourcingId",
                table: "LeadBusinessAchievements");

            migrationBuilder.DropColumn(
                name: "ProjectStatusHandOverRemarks",
                table: "LeadFollowUps");

            migrationBuilder.DropColumn(
                name: "ProductSourcingId",
                table: "LeadBusinessAchievements");

            migrationBuilder.DropColumn(
                name: "ProductSourcingRemarks",
                table: "LeadBusinessAchievements");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldExteriorGallon",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldExteriorKg",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldInteriorGallon",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldInteriorKg",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldTopCoatGallon",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualVolumeSoldUnderCoatGallon",
                table: "LeadFollowUps",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductSourcing",
                table: "LeadBusinessAchievements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
