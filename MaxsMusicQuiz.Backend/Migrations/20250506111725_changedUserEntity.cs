using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class changedUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "GameHistories",
                newName: "QuestionsAnswered");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsAnswered",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "GameHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuestionsAnswered",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "GameHistories");

            migrationBuilder.RenameColumn(
                name: "QuestionsAnswered",
                table: "GameHistories",
                newName: "Score");
        }
    }
}
