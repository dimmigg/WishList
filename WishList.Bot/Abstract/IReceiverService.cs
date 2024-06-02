namespace WishList.Bot.Abstract;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}
