using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WishList.Storage.Migrations
{
    /// <inheritdoc />
    public partial class UserSubscribe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUserWishList_WishLists_ReadWishListsId",
                table: "TelegramUserWishList");

            migrationBuilder.RenameColumn(
                name: "ReadWishListsId",
                table: "TelegramUserWishList",
                newName: "SubscribeWishListsId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserWishList_ReadWishListsId",
                table: "TelegramUserWishList",
                newName: "IX_TelegramUserWishList_SubscribeWishListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUserWishList_WishLists_SubscribeWishListsId",
                table: "TelegramUserWishList",
                column: "SubscribeWishListsId",
                principalTable: "WishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramUserWishList_WishLists_SubscribeWishListsId",
                table: "TelegramUserWishList");

            migrationBuilder.RenameColumn(
                name: "SubscribeWishListsId",
                table: "TelegramUserWishList",
                newName: "ReadWishListsId");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramUserWishList_SubscribeWishListsId",
                table: "TelegramUserWishList",
                newName: "IX_TelegramUserWishList_ReadWishListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramUserWishList_WishLists_ReadWishListsId",
                table: "TelegramUserWishList",
                column: "ReadWishListsId",
                principalTable: "WishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
