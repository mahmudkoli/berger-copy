using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class ELearningUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "QuestionSets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "QuestionSets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TimeOutMinute",
                table: "QuestionSets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionSetDepots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionSetId = table.Column<int>(nullable: false),
                    Depot = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSetDepots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionSetDepots_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSetDepots_QuestionSetId",
                table: "QuestionSetDepots",
                column: "QuestionSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionSetDepots");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "QuestionSets");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "QuestionSets");

            migrationBuilder.DropColumn(
                name: "TimeOutMinute",
                table: "QuestionSets");
        }
    }
}
