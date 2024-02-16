using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Start;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.CommandOptions;

namespace WishList.Domain.Received;

public class ReceivedService(
    ISender sender,
    IUpdateUserUseCase updateUserUseCase,
    ICallbackQueryBuilder callbackQueryBuilder,
    IStartUseCase startUseCase
    ) : IReceivedService
{
    public async Task MessageReceivedAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText||
            message.From is not { } tgUser)
            return;

        var user = await updateUserUseCase.CreateOrUpdateUser(tgUser, cancellationToken);
        if(user.Command != Command.Null && user.CommandStep != CommandStep.Null)
        {
            var callbackReceived = callbackQueryBuilder.Build(user.Command, user.CommandStep, cancellationToken);
            await callbackReceived.Execute(message, cancellationToken);
        }
        else
        {
            var action = messageText.Split(' ')[0] switch
            {
                "/start" => startUseCase.Execute(message, user, cancellationToken),
                _ => Usage(message, cancellationToken)
            };
            await action;
        }
    }
    
    public async Task CallbackQueryReceivedAsync(CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if(callbackQuery.Data == null) return;
        
        callbackQuery.ParseCommand(out var way, out var step);
        
        var callbackReceived = callbackQueryBuilder.Build(way, step, cancellationToken);
        
        await callbackReceived.Execute(callbackQuery, cancellationToken);
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

        await sender.AnswerInlineQueryAsync(
            inlineQueryId: inlineQuery.Id,
            results: results,
            cacheTime: 0,
            isPersonal: true,
            cancellationToken: cancellationToken);
    }
    
    public async Task ChosenInlineResultReceivedAsync(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
    {
        await sender.SendTextMessageAsync(
            chatId: chosenInlineResult.From.Id,
            text: $"You chose result with Id: {chosenInlineResult.ResultId}",
            cancellationToken: cancellationToken);
    }
    
    public Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async Task<Message> Usage(Message message, CancellationToken cancellationToken)
    {
        const string usage = "Что-то хотели?";

        return await sender.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
}