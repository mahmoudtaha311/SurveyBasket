using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class altercolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerS_Question_questionId",
                table: "AnswerS");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Question",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "AnswerS",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerS_questionId_Content",
                table: "AnswerS",
                newName: "IX_AnswerS_QuestionId_Content");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerS_Question_QuestionId",
                table: "AnswerS",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerS_Question_QuestionId",
                table: "AnswerS");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Question",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "AnswerS",
                newName: "questionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerS_QuestionId_Content",
                table: "AnswerS",
                newName: "IX_AnswerS_questionId_Content");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerS_Question_questionId",
                table: "AnswerS",
                column: "questionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
