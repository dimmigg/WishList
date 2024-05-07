using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Users.UsersFind;

public class UsersFindCommand(UseCaseParam param) : CommandBase(param);