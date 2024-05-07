using WishList.Domain.Models;

namespace WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishListRequest;

public class UnsubscribeWishListRequestCommand(UseCaseParam param) : CommandBase(param);