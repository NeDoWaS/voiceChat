using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceChat.Server.Migrations
{
    /// <inheritdoc />
    public partial class lalalend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateSent",
                table: "RoomMessages",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "RoomMessages",
                newName: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_Token",
                table: "UserSessions",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_Token",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "RoomMessages",
                newName: "DateSent");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "RoomMessages",
                newName: "AuthorId");
        }
    }
}
