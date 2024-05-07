using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishLists;

public class MyWishListsCommand(UseCaseParam param) : CommandBase(param);