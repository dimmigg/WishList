using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.CommandOptions;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Received.CallbackQueryReceived.CreateWishList;

public class CreateWishListCallbackReceived(
    Command command,
    CommandStep commandStep,
    IUserStorage userStorage,
    IWishListStorage wishListStorage,
    ISender sender)
    : ICallbackReceived
{
    public async Task Execute(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        switch (commandStep)
        {
            case CommandStep.First:
                await RequestListName(callbackQuery, cancellationToken);
                break;
            default:
                throw new DomainException("Команда не распознана");
        }
    }

    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        switch (commandStep)
        {
            case CommandStep.Second:
                await AddWishList(message, cancellationToken);
                break;
            default:
                throw new DomainException("Команда не распознана");
        }
    }

    private async Task AddWishList(Message message, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(message.Text)) throw new DomainException("Пустое название WishList недопустимо");
        if(message.From == null) throw new DomainException("Пользователь не может быть пустым");
        await wishListStorage.AddWishList(message.Text, message.From.Id, cancellationToken);

        var textMessage = $"WishList {message.Text} успешно добавлен!";
        
        await sender.SendTextMessageAsync(
            chatId: message.Chat.Id,
            replyMarkup: InlineKeyboardMarkup.Empty(),
            text: textMessage,
            cancellationToken: cancellationToken);
        
        await userStorage.UpdateWayUser(message.From.Id, Command.Null, CommandStep.Null, cancellationToken);
    }

    private async Task RequestListName(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите название списка";

        await sender.DeleteMessageAsync(
            messageId: callbackQuery.Message!.MessageId,
            chatId: callbackQuery.Message.Chat.Id,
            cancellationToken: cancellationToken
        );
        
        await sender.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: textMessage,
            cancellationToken: cancellationToken);
        
        await sender.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            replyMarkup: InlineKeyboardMarkup.Empty(),
            text: textMessage,
            cancellationToken: cancellationToken);
        await userStorage.UpdateWayUser(callbackQuery.From.Id, command, CommandStep.Second, cancellationToken);
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