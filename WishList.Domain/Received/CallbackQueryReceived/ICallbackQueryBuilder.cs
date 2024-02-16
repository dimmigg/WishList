using WishList.Storage.CommandOptions;

namespace WishList.Domain.Received.CallbackQueryReceived;

public interface ICallbackQueryBuilder
{
    ICallbackReceived Build(Command way, CommandStep step, CancellationToken cancellationToken);
}