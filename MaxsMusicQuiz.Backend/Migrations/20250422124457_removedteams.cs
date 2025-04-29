using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class removedteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "QuizGameUsers");

            migrationBuilder.DropColumn(
                name: "TeamAScore",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "TeamBScore",
                table: "QuizGames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "QuizGameUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TeamAScore",
                table: "QuizGames",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamBScore",
                table: "QuizGames",
                type: "int",
                nullable: true);
        }
    }
}
