using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Users.UsersFindRequest;

public class UsersFindRequestCommand(UseCaseParam param) : CommandBase(param);