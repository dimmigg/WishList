using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Other.SelfDeleteButton;

public class SelfDeleteButtonCommand(UseCaseParam param) : CommandBase(param);