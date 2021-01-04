using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class QuestionSetUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ELearningDocumentId",
                table: "QuestionSets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSets_ELearningDocumentId",
                table: "QuestionSets",
                column: "ELearningDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionSets_ELearningDocuments_ELearningDocumentId",
                table: "QuestionSets",
                column: "ELearningDocumentId",
                principalTable: "ELearningDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionSets_ELearningDocuments_ELearningDocumentId",
                table: "QuestionSets");

            migrationBuilder.DropIndex(
                name: "IX_QuestionSets_ELearningDocumentId",
                table: "QuestionSets");

            migrationBuilder.DropColumn(
                name: "ELearningDocumentId",
                table: "QuestionSets");
        }
    }
}
