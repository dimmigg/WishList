using MediatR;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.Other.SelfDeleteButton;

public class SelfDeleteButtonUseCase(
    ITelegramSender telegramSender)
    : IRequestHandler<SelfDeleteButtonCommand>
{
    public async Task Handle(SelfDeleteButtonCommand request, CancellationToken cancellationToken)
    {
        await telegramSender.AnswerCallbackQueryAsync(
            text: "Ввод отменён",
            cancellationToken: cancellationToken);
        
        await telegramSender.DeleteMessageAsync(cancellationToken: cancellationToken);
    }
}