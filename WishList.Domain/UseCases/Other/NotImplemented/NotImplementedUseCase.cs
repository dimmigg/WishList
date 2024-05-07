using MediatR;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Other.NotImplemented;

public class NotImplementedUseCase(
    ISender sender)
    : IRequestHandler<NotImplementedCommand>
{
    public async Task Handle(NotImplementedCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.AnswerCallbackQueryAsync(
                callbackQueryId: request.Param.CallbackQuery.Id,
                text: "Функционал еще не реализован 🙄",
                showAlert: true,
                cancellationToken: cancellationToken);
        }
    }
}