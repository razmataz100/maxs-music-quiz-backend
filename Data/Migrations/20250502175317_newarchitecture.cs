using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class newarchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizGameUsers");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "QuizGames");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "QuizGames");

            migrationBuilder.CreateTable(
                name: "GameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizGameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameHistories_QuizGames_QuizGameId",
                        column: x => x.QuizGameId,
                        principalTable: "QuizGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameHistories_QuizGameId",
                table: "GameHistories",
                column: "QuizGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameHistories_UserId",
                table: "GameHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameHistories");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "QuizGames",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "QuizGames",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "QuizGames",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuizGameUsers",
                columns: table => new
                {
                    QuizGameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizGameUsers", x => new { x.QuizGameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuizGameUsers_QuizGames_QuizGameId",
                        column: x => x.QuizGameId,
                        principalTable: "QuizGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizGameUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizGameUsers_UserId",
                table: "QuizGameUsers",
                column: "UserId");
        }
    }
}
