using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaxsMusicQuiz.Backend.Migrations
{
    /// <inheritdoc />
    public partial class someupdates4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "Users",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Nickname");
        }
    }
}
