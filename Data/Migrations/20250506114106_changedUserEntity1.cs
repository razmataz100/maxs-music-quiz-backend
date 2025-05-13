using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class changedUserEntity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionsAnswered",
                table: "Users",
                newName: "TotalScore");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswers",
                table: "Users",
                newName: "TotalQuestionsAnswered");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalScore",
                table: "Users",
                newName: "QuestionsAnswered");

            migrationBuilder.RenameColumn(
                name: "TotalQuestionsAnswered",
                table: "Users",
                newName: "CorrectAnswers");
        }
    }
}
