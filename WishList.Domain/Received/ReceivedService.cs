using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.UseCases.Start;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.Received;

public class ReceivedService(
    ITelegramBotClient botClient,
    IUpdateUserUseCase updateUserUseCase,
    IStartUseCase startUseCase
    ) : IReceivedService
{
    public async Task MessageReceivedAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText)
            return;

        await updateUserUseCase.CreateOrUpdateUser(message.From, cancellationToken);
        
        var action = messageText.Split(' ')[0] switch
        {
            "/start" => startUseCase.Execute(message, cancellationToken),
            _ => Usage(message, cancellationToken)
        };
        var sentMessage = await action;
        return;
        
        async Task<Message> Usage(Message message, CancellationToken cancellationToken)
        {
            const string usage = "Usage:\n" +
                                 "/inline_keyboard - send inline keyboard\n" +
                                 "/keyboard    - send custom keyboard\n" +
                                 "/remove      - remove custom keyboard\n" +
                                 "/photo       - send a photo\n" +
                                 "/request     - request location or contact\n" +
                                 "/inline_mode - send keyboard with Inline Query";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
    
    public async Task CallbackQueryReceivedAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);
    }
    
    public async Task InlineQueryReceivedAsync(InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent("hello"))
        };

        await botClient.AnswerInlineQueryAsync(
            inlineQueryId: inlineQuery.Id,
            results: results,
            cacheTime: 0,
            isPersonal: true,
            cancellationToken: cancellationToken);
    }
    
    public async Task ChosenInlineResultReceivedAsync(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: chosenInlineResult.From.Id,
            text: $"You chose result with Id: {chosenInlineResult.ResultId}",
            cancellationToken: cancellationToken);
    }
    
    public Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}