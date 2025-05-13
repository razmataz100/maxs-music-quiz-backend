using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class minorfixtoentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "QuizGameId",
                table: "QuizQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions",
                column: "QuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "QuizGameId",
                table: "QuizQuestions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_QuizGames_QuizGameId",
                table: "QuizQuestions",
                column: "QuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id");
        }
    }
}
