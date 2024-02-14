using Telegram.Bot.Types;
using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Start;

public interface IStartUseCase
{
    public Task Execute(Message message, RegisteredUser user, CancellationToken cancellationToken);
}