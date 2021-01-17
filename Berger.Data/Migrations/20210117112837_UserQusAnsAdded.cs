using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class UserQusAnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserQuestionAnswers",
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
                    UserInfoId = table.Column<int>(nullable: false),
                    QuestionSetId = table.Column<int>(nullable: false),
                    TotalMark = table.Column<int>(nullable: false),
                    TotalCorrectAnswer = table.Column<int>(nullable: false),
                    Passed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswers_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswers_UserInfos_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestionAnswerCollections",
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
                    UserQuestionAnswerId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false),
                    IsCorrectAnswer = table.Column<bool>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionAnswerCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswerCollections_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestionAnswerCollections_UserQuestionAnswers_UserQuestionAnswerId",
                        column: x => x.UserQuestionAnswerId,
                        principalTable: "UserQuestionAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswerCollections_QuestionId",
                table: "UserQuestionAnswerCollections",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswerCollections_UserQuestionAnswerId",
                table: "UserQuestionAnswerCollections",
                column: "UserQuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswers_QuestionSetId",
                table: "UserQuestionAnswers",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionAnswers_UserInfoId",
                table: "UserQuestionAnswers",
                column: "UserInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuestionAnswerCollections");

            migrationBuilder.DropTable(
                name: "UserQuestionAnswers");
        }
    }
}
