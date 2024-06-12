using MediatR;
using Telegram.Bot.Types;
using WishList.Domain.Models;
using WishList.Domain.UseCases.Builder;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.Received;

public class ReceivedService(
    IUseCaseBuilder useCaseBuilder,
    IUpdateUserUseCase updateUserUseCase,
    IMediator mediator
    ) : IReceivedService
{
    public async Task MessageReceivedAsync(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText||
            message.From is not { } tgUser)
            return;

        var user = await updateUserUseCase.CreateOrUpdateUserAsync(tgUser, cancellationToken);
        var param = new UseCaseParam
        {
            User = user,
            Command = string.IsNullOrWhiteSpace(user.LastCommand) ? "main" : user.LastCommand,
            Message = message
        };
        
        await ExecuteUseCaseAsync(param, cancellationToken);
    }
    
    public async Task CallbackQueryReceivedAsync(CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if(callbackQuery.Data is not { } messageText||
           callbackQuery.From is not { } tgUser) return;
        
        var user = await updateUserUseCase.CreateOrUpdateUserAsync(tgUser, cancellationToken);
        updateUserUseCase.UpdateLastCommandUser(user.Id);
        var param = new UseCaseParam
        {
            User = user,
            Command = callbackQuery.Data,
            CallbackQuery = callbackQuery
        };
        
        await ExecuteUseCaseAsync(param, cancellationToken);
    }

    private async Task ExecuteUseCaseAsync(UseCaseParam param, CancellationToken cancellationToken)
    {
        var request = useCaseBuilder.Build(param);
        await mediator.Send(request, cancellationToken);
    }
    
    public Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}