using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishList.Storage.Migrations
{
    /// <inheritdoc />
    public partial class PresentAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Presents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Presents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Presents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Presents");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Presents");
        }
    }
}
