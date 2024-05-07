using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribeRequest;

public class UserWishListSubscribeRequestCommand(UseCaseParam param) : CommandBase(param);