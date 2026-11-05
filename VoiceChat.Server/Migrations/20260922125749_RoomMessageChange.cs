using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceChat.Server.Migrations
{
    /// <inheritdoc />
    public partial class RoomMessageChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RoomMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RoomMessages");
        }
    }
}
