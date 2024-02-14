namespace WishList.Domain.Received.CallbackQueryReceived;

public interface ICallbackQueryBuilder
{
    ICallbackReceived Build(string command, CancellationToken cancellationToken);
}