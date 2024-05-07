using WishList.Domain.Models;

namespace WishList.Domain.UseCases.MyPresents.MyPresentAddRequest;

public class MyPresentAddRequestCommand(UseCaseParam param) : CommandBase(param);