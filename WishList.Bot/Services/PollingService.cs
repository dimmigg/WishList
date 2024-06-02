using WishList.Bot.Abstract;

namespace WishList.Bot.Services;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
