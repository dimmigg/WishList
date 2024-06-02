using MediatR;
using Microsoft.Extensions.Logging;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases;

namespace WishList.Domain.PipelineBehavior;

public class ServiceFiller<TRequest, TResponse>(
    ITelegramSender telegramSender,
    ILogger<ServiceFiller<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started command {Command}", request);
        if (request is not CommandBase command) return await next.Invoke();
        telegramSender.MessageId = command.Param.CallbackQuery?.Message?.MessageId;
        telegramSender.ChatId = command.Param.Message?.Chat.Id ?? command.Param.CallbackQuery!.Message!.Chat.Id;
        telegramSender.CallbackQueryId = command.Param.CallbackQuery?.Id;

        try
        {
            await telegramSender.AnswerCallbackQueryAsync(null, cancellationToken: cancellationToken);
            var response = await next.Invoke();
            logger.LogInformation("Command successfully handled {Command}", request);
            return response;
        }
        catch (DomainException e)
        {
            await telegramSender.ShowAlertAsync(e.Message, cancellationToken);
            
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled error caught while handling command {Command}", request);
            throw;
        }
    }
}