using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelServer.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Hotel");

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "Hotel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TypeHotel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeHotel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hotel_TypeId",
                table: "Hotel",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_TypeHotel_TypeId",
                table: "Hotel",
                column: "TypeId",
                principalTable: "TypeHotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_TypeHotel_TypeId",
                table: "Hotel");

            migrationBuilder.DropTable(
                name: "TypeHotel");

            migrationBuilder.DropIndex(
                name: "IX_Hotel_TypeId",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Hotel");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Hotel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
