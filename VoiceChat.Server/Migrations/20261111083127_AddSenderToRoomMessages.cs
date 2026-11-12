using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceChat.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderToRoomMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "RoomMessages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "RoomMessages");
        }
    }
}
