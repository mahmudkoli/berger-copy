using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class MerchandisingSnapshotAndLoginLogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessArea",
                table: "EmailConfigForDealerOppenings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FCMToken = table.Column<string>(nullable: true),
                    IsLoggedIn = table.Column<bool>(nullable: false),
                    LoggedInTime = table.Column<DateTime>(nullable: false),
                    LoggedOutTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchandisingSnapShots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MerchandisingSnapShotCategoryId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchandisingSnapShots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchandisingSnapShots_DealerInfos_DealerId",
                        column: x => x.DealerId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchandisingSnapShots_DropdownDetails_MerchandisingSnapShotCategoryId",
                        column: x => x.MerchandisingSnapShotCategoryId,
                        principalTable: "DropdownDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchandisingSnapShots_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfigForDealerSalesCalls_DealerSalesIssueCategoryId",
                table: "EmailConfigForDealerSalesCalls",
                column: "DealerSalesIssueCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_UserId",
                table: "LoginLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandisingSnapShots_DealerId",
                table: "MerchandisingSnapShots",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandisingSnapShots_MerchandisingSnapShotCategoryId",
                table: "MerchandisingSnapShots",
                column: "MerchandisingSnapShotCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandisingSnapShots_UserId",
                table: "MerchandisingSnapShots",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailConfigForDealerSalesCalls_DropdownDetails_DealerSalesIssueCategoryId",
                table: "EmailConfigForDealerSalesCalls",
                column: "DealerSalesIssueCategoryId",
                principalTable: "DropdownDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailConfigForDealerSalesCalls_DropdownDetails_DealerSalesIssueCategoryId",
                table: "EmailConfigForDealerSalesCalls");

            migrationBuilder.DropTable(
                name: "LoginLogs");

            migrationBuilder.DropTable(
                name: "MerchandisingSnapShots");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfigForDealerSalesCalls_DealerSalesIssueCategoryId",
                table: "EmailConfigForDealerSalesCalls");

            migrationBuilder.DropColumn(
                name: "BusinessArea",
                table: "EmailConfigForDealerOppenings");
        }
    }
}
