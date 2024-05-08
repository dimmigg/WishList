using MediatR;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases;

namespace WishList.Domain.PipelineBehavior;

public class ServiceFiller<TRequest, TResponse>(ITelegramSender sender)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not CommandBase command) return await next.Invoke();
        sender.MessageId = command.Param.CallbackQuery?.Message?.MessageId;
        sender.ChatId = command.Param.Message?.Chat.Id ?? command.Param.CallbackQuery!.Message!.Chat.Id;
        sender.CallbackQueryId = command.Param.CallbackQuery?.Id;

        return await next.Invoke();
    }
}