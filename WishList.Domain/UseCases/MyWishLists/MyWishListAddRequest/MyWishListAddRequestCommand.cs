using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;

public class MyWishListAddRequestCommand(UseCaseParam param) : CommandBase(param);