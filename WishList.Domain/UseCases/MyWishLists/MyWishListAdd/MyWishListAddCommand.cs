using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAdd;

public class MyWishListAddCommand(UseCaseParam param) : CommandBase(param);