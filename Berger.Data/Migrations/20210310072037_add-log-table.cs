using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class addlogtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandInfoStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BrandInfoId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandInfoStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandInfoStatusLogs_BrandInfos_BrandInfoId",
                        column: x => x.BrandInfoId,
                        principalTable: "BrandInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandInfoStatusLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerInfoStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DealerInfoId = table.Column<int>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInfoStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerInfoStatusLogs_DealerInfos_DealerInfoId",
                        column: x => x.DealerInfoId,
                        principalTable: "DealerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerInfoStatusLogs_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandInfoStatusLogs_BrandInfoId",
                table: "BrandInfoStatusLogs",
                column: "BrandInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandInfoStatusLogs_UserId",
                table: "BrandInfoStatusLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfoStatusLogs_DealerInfoId",
                table: "DealerInfoStatusLogs",
                column: "DealerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfoStatusLogs_UserId",
                table: "DealerInfoStatusLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandInfoStatusLogs");

            migrationBuilder.DropTable(
                name: "DealerInfoStatusLogs");
        }
    }
}
