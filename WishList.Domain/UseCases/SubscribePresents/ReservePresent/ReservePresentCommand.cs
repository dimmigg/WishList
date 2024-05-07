using WishList.Domain.Models;

namespace WishList.Domain.UseCases.SubscribePresents.ReservePresent;

public class ReservePresentCommand(UseCaseParam param) : CommandBase(param);