using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizQuestionPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_QuizGames_GameId",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_GameId",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "QuizQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerChoices",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuizGameId",
                table: "QuizQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizGameId",
                table: "QuizQuestions",
                column: "QuizGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions",
                column: "QuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_QuizGameId",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "AnswerChoices",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "QuizGameId",
                table: "QuizQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "QuizQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_GameId",
                table: "QuizQuestions",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_QuizGames_GameId",
                table: "QuizQuestions",
                column: "GameId",
                principalTable: "QuizGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
