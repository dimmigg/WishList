using Telegram.Bot.Types;

namespace WishList.Domain.UseCases.AddWishList;

public interface ISuggestAddingUseCase
{
    public Task Execute(Message message, CancellationToken cancellationToken);
}