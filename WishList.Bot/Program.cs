using WishList.Bot;
using WishList.Bot.Controllers;
using WishList.Bot.Services;
using Telegram.Bot;
using WishList.Domain.DependencyInjection;
using WishList.Domain.Received;
using WishList.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>() ??
                       throw new InvalidOperationException("BotConfiguration not found.");
var connectionString = builder.Configuration.GetConnectionString("Postgres") ??
                       throw new InvalidOperationException("Connection string for 'Postgres' not found.");

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        var botConfig = sp.GetConfiguration<BotConfiguration>();
        TelegramBotClientOptions options = new(botConfig.BotToken);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddScoped<IReceivedService, ReceivedService>();
builder.Services.AddScoped<UpdateHandlers>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();
builder.Services.AddMemoryCache();


builder.Services
    .AddStorage(connectionString)
    .AddDomain()
    .AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();
app.UseHealthChecks("/health");
app.Run();