using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class QuestionSetAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    TotalMark = table.Column<int>(nullable: false),
                    PassMark = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionSetCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<int>(nullable: true),
                    WFStatus = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSetCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionSetCollections_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionSetCollections_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSetCollections_QuestionId",
                table: "QuestionSetCollections",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSetCollections_QuestionSetId",
                table: "QuestionSetCollections",
                column: "QuestionSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionSetCollections");

            migrationBuilder.DropTable(
                name: "QuestionSets");
        }
    }
}
