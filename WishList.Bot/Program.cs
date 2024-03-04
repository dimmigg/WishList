using WishList.Bot;
using WishList.Bot.Controllers;
using WishList.Bot.Services;
using Telegram.Bot;
using WishList.Domain.DependencyInjection;
using WishList.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();
var connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        var botConfig = sp.GetConfiguration<BotConfiguration>();
        TelegramBotClientOptions options = new(botConfig.BotToken);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddScoped<UpdateHandlers>();
builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services
    .AddStorage(connectionString)
    .AddDomain()
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.MapBotWebhookRoute<BotController>(route: botConfiguration?.Route);
app.MapControllers();
app.Run();