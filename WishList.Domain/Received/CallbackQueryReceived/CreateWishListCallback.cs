using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.WayOptions;

namespace WishList.Domain.Received.CallbackQueryReceived;

public class CreateWishListCallbackReceived(
    Way way,
    StepWay step,
    IUserStorage userStorage,
    ISender sender)
    : ICallbackReceived
{
    public async Task Execute(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await userStorage.UpdateWayUser(callbackQuery.From.Id, way, step, cancellationToken);
        switch (step)
        {
            case StepWay.First:
                await RequestListName(callbackQuery, cancellationToken);
                break;
            case StepWay.Second:
                break;
            case StepWay.Third:
                break;
            case StepWay.Fourth:
                break;
            case StepWay.Fifth:
                break;
            case StepWay.Null:
            default:
                throw new DomainException("Команда не распознана");
        }
    }

    private async Task RequestListName(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите название списка";
        await sender.EditMessageTextAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: InlineKeyboardMarkup.Empty(),
            text: textMessage,
            cancellationToken: cancellationToken);
    }
}