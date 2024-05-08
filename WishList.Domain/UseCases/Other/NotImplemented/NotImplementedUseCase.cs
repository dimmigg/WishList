using MediatR;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.Other.NotImplemented;

public class NotImplementedUseCase(
    ITelegramSender telegramSender)
    : IRequestHandler<NotImplementedCommand>
{
    public Task Handle(NotImplementedCommand request, CancellationToken cancellationToken) =>
        telegramSender.AnswerCallbackQueryAsync(
            text: "Функционал еще не реализован 🙄",
            showAlert: true,
            cancellationToken: cancellationToken);
}