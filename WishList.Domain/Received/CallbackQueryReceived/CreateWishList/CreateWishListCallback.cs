using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.CommandOptions;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Received.CallbackQueryReceived.CreateWishList;

public class CreateWishListCallbackReceived(
    Command command,
    CommandStep commandStep,
    IUserStorage userStorage,
    ISender sender)
    : ICallbackReceived
{
    public async Task Execute(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await userStorage.UpdateWayUser(callbackQuery.From.Id, command, commandStep, cancellationToken);
        switch (commandStep)
        {
            case CommandStep.First:
                await RequestListName(callbackQuery, cancellationToken);
                break;
            case CommandStep.Second:
                break;
            case CommandStep.Third:
                break;
            case CommandStep.Fourth:
                break;
            case CommandStep.Fifth:
                break;
            case CommandStep.Null:
            default:
                throw new DomainException("Команда не распознана");
        }
    }

    private async Task RequestListName(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите название списка";
        
        await sender.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: textMessage,
            cancellationToken: cancellationToken);
        
        await sender.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            replyMarkup: InlineKeyboardMarkup.Empty(),
            text: textMessage,
            cancellationToken: cancellationToken);
    }

    public void SetWay(Command newCommand)
    {
        command = newCommand;
    }

    public void SetStepWay(CommandStep newCommandStep)
    {
        commandStep = newCommandStep;
    }
}