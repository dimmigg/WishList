using WishList.Domain.Models;

namespace WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishList;

public class UnsubscribeWishListCommand(UseCaseParam param) : CommandBase(param);