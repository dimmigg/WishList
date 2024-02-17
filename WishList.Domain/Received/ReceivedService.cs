using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Builder;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.Received;

public class ReceivedService(
    ISender sender,
    IUseCaseBuilder useCaseBuilder,
    IUpdateUserUseCase updateUserUseCase
    ) : IReceivedService
{
    public async Task MessageReceivedAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText||
            message.From is not { } tgUser)
            return;

        var user = await updateUserUseCase.CreateOrUpdateUser(tgUser, cancellationToken);
        var param = new BuildParam
        {
            User = user,
            Command = string.IsNullOrWhiteSpace(user.LastCommand) ? "main" : user.LastCommand,
            Message = message
        };
        var useCase = useCaseBuilder.Build(param);
        await useCase.Execute(cancellationToken);
    }
    
    public async Task CallbackQueryReceivedAsync(CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if(callbackQuery.Data is not { } messageText||
           callbackQuery.From is not { } tgUser) return;
        
        var user = await updateUserUseCase.CreateOrUpdateUser(tgUser, cancellationToken);
        var param = new BuildParam
        {
            User = user,
            Command = callbackQuery.Data,
            CallbackQuery = callbackQuery
        };
        var useCase = useCaseBuilder.Build(param);
        await useCase.Execute(cancellationToken);
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
}