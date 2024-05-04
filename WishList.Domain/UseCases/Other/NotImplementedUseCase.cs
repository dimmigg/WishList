using WishList.Domain.Models;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.Other;

public class NotImplementedUseCase(
    UseCaseParam param,
    ISender sender)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        var command = param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var chatId = param.CallbackQuery.Message?.Chat.Id;
            var messageId = param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.AnswerCallbackQueryAsync(
                callbackQueryId: param.CallbackQuery.Id,
                text: "Функционал еще не реализован 🙄",
                showAlert: true,
                cancellationToken: cancellationToken);
        }
    }
}