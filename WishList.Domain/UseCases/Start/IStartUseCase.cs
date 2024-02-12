using Telegram.Bot.Types;

namespace WishList.Domain.UseCases.Start;

public interface IStartUseCase
{
    public Task<Message> Execute(Message message, CancellationToken cancellationToken);
}