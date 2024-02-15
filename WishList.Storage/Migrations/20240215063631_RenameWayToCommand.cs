using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishList.Storage.Migrations
{
    /// <inheritdoc />
    public partial class RenameWayToCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WayStep",
                table: "Users",
                newName: "CommandStep");

            migrationBuilder.RenameColumn(
                name: "CurrentWay",
                table: "Users",
                newName: "Command");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommandStep",
                table: "Users",
                newName: "WayStep");

            migrationBuilder.RenameColumn(
                name: "Command",
                table: "Users",
                newName: "CurrentWay");
        }
    }
}
