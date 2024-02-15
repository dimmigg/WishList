using Telegram.Bot.Types;

namespace WishList.Domain.UseCases.SuggestAddWishList;

public interface ISuggestAddWishListUseCase
{
    public Task Execute(Message message, CancellationToken cancellationToken);
}