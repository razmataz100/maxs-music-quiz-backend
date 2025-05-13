using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureQuizGameUserRelationship3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizGameUser_QuizGames_QuizGameId",
                table: "QuizGameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGameUser_Users_UserId",
                table: "QuizGameUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizGameUser",
                table: "QuizGameUser");

            migrationBuilder.RenameTable(
                name: "QuizGameUser",
                newName: "QuizGameUsers");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGameUser_UserId",
                table: "QuizGameUsers",
                newName: "IX_QuizGameUsers_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Team",
                table: "QuizGameUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizGameUsers",
                table: "QuizGameUsers",
                columns: new[] { "QuizGameId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGameUsers_QuizGames_QuizGameId",
                table: "QuizGameUsers",
                column: "QuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGameUsers_Users_UserId",
                table: "QuizGameUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizGameUsers_QuizGames_QuizGameId",
                table: "QuizGameUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizGameUsers_Users_UserId",
                table: "QuizGameUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizGameUsers",
                table: "QuizGameUsers");

            migrationBuilder.RenameTable(
                name: "QuizGameUsers",
                newName: "QuizGameUser");

            migrationBuilder.RenameIndex(
                name: "IX_QuizGameUsers_UserId",
                table: "QuizGameUser",
                newName: "IX_QuizGameUser_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Team",
                table: "QuizGameUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizGameUser",
                table: "QuizGameUser",
                columns: new[] { "QuizGameId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGameUser_QuizGames_QuizGameId",
                table: "QuizGameUser",
                column: "QuizGameId",
                principalTable: "QuizGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizGameUser_Users_UserId",
                table: "QuizGameUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
