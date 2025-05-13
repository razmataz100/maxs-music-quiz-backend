using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RenamePreviewUrlToSpotifyTrackId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuizGames");

            migrationBuilder.RenameColumn(
                name: "PreviewUrl",
                table: "QuizQuestions",
                newName: "SpotifyTrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpotifyTrackId",
                table: "QuizQuestions",
                newName: "PreviewUrl");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "QuizGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
