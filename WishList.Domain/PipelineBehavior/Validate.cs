using MediatR;
using WishList.Domain.Exceptions;
using WishList.Domain.UseCases;

namespace WishList.Domain.PipelineBehavior;

public class Validate<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not CommandBase command) return await next.Invoke();

        var hasMessage = command.Param.Message != null && !string.IsNullOrWhiteSpace(command.Param.Message.Text);
        var hasCallbackQuery = command.Param.CallbackQuery != null;

        if (!hasMessage && !hasCallbackQuery)
            throw new DomainException("Нет сообщения или коллбэк-запроса для обработки команды.");

        return await next.Invoke();
    }
}