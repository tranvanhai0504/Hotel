using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelServer.Migrations
{
    /// <inheritdoc />
    public partial class updateBdAddRoomtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Room");

            migrationBuilder.AddColumn<string>(
                name: "QuantityId",
                table: "Room",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TypeRoom",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeRoom", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_QuantityId",
                table: "Room",
                column: "QuantityId");

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

            migrationBuilder.DropTable(
                name: "TypeRoom");

            migrationBuilder.DropIndex(
                name: "IX_Room_QuantityId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "QuantityId",
                table: "Room");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
