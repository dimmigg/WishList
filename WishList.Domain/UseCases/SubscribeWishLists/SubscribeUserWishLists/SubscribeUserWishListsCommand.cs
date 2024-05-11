using WishList.Domain.Models;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeUserWishLists;

public class SubscribeUserWishListsCommand(UseCaseParam param) : CommandBase(param);