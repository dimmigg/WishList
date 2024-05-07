using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListParams;

public class MyWishListParamsCommand(UseCaseParam param) : CommandBase(param);