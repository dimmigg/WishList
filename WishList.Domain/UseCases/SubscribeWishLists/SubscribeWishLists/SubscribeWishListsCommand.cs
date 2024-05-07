using WishList.Domain.Models;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishLists;

public class SubscribeWishListsCommand(UseCaseParam param) : CommandBase(param);