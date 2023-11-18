using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelServer.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_TypeRooms_QuantityId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeRooms",
                table: "TypeRooms");

            migrationBuilder.RenameTable(
                name: "TypeRooms",
                newName: "TypeRoom");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeRoom",
                table: "TypeRoom",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_TypeRoom_QuantityId",
                table: "Room",
                column: "QuantityId",
                principalTable: "TypeRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_TypeRoom_QuantityId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeRoom",
                table: "TypeRoom");

            migrationBuilder.RenameTable(
                name: "TypeRoom",
                newName: "TypeRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeRooms",
                table: "TypeRooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_TypeRooms_QuantityId",
                table: "Room",
                column: "QuantityId",
                principalTable: "TypeRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
