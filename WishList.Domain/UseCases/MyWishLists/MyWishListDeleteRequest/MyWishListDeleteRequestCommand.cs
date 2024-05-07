using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListDeleteRequest;

public class MyWishListDeleteRequestCommand(UseCaseParam param) : CommandBase(param);