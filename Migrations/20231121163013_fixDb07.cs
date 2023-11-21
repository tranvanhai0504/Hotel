using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelServer.Migrations
{
    /// <inheritdoc />
    public partial class fixDb07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_TypeRoom_QuantityId",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "QuantityId",
                table: "Room",
                newName: "TypeRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_QuantityId",
                table: "Room",
                newName: "IX_Room_TypeRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_TypeRoom_TypeRoomId",
                table: "Room",
                column: "TypeRoomId",
                principalTable: "TypeRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_TypeRoom_TypeRoomId",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "TypeRoomId",
                table: "Room",
                newName: "QuantityId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_TypeRoomId",
                table: "Room",
                newName: "IX_Room_QuantityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_TypeRoom_QuantityId",
                table: "Room",
                column: "QuantityId",
                principalTable: "TypeRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
