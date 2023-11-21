using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelServer.Migrations
{
    /// <inheritdoc />
    public partial class fixDb04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Utilities",
                table: "Hotel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Utilities",
                table: "Hotel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
