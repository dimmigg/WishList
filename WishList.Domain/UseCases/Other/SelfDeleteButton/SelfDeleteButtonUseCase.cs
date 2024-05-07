using MediatR;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Other.SelfDeleteButton;

public class SelfDeleteButtonUseCase(
    ISender sender)
    : IRequestHandler<SelfDeleteButtonCommand>
{
    public async Task Handle(SelfDeleteButtonCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;
        
        var chatId = request.Param.CallbackQuery?.Message?.Chat.Id;
        var messageId = request.Param.CallbackQuery?.Message?.MessageId;
        if (!chatId.HasValue || !messageId.HasValue) return;
        
        await sender.AnswerCallbackQueryAsync(
            callbackQueryId: request.Param.CallbackQuery.Id,
            text: "Ввод отменён",
            cancellationToken: cancellationToken);
        
        await sender.DeleteMessageAsync(
            chatId: chatId.Value,
            messageId: messageId.Value,
            cancellationToken: cancellationToken);
    }
}