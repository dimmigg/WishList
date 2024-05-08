using MediatR;
using WishList.Domain.Exceptions;
using WishList.Domain.UseCases;

namespace WishList.Domain.PipelineBehavior;

public class Validate<TRequest, TResponse>()
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not CommandBase command) return await next.Invoke();
        
        if (command.Param.Message == null && 
            string.IsNullOrWhiteSpace(command.Param.Message?.Text) &&
            command.Param.CallbackQuery == null)
            throw new DomainException("Параметы команды не валидны");
        
        return await next.Invoke();
    }
}