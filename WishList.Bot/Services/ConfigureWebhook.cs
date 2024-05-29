using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WishList.Bot.Services;

public class ConfigureWebhook(
    ILogger<ConfigureWebhook> logger,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IOptions<BotConfiguration> botOptions)
    : IHostedService
{
    private readonly BotConfiguration _botConfig = botOptions.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        var webhookAddress = $"{_botConfig.HostAddress}{_botConfig.Route}";
        logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            secretToken: _botConfig.SecretToken,
            cancellationToken: cancellationToken);

        var cs = configuration.GetConnectionString("Postgres") ?? "не получилось";

        await botClient.SendTextMessageAsync(
            chatId: 252395776,
            cs.MarkForbiddenChar(),
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        // Remove webhook on app shutdown
        logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}