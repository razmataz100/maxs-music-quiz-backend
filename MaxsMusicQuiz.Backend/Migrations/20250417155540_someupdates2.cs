using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class someupdates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "QuizGames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "QuizGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuizGames");
        }
    }
}
