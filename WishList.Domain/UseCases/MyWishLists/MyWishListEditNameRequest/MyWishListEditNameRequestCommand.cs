using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditNameRequest;

public class MyWishListEditNameRequestCommand(UseCaseParam param) : CommandBase(param);