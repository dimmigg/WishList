using AutoMapper;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using WishList.Storage.WayOptions;

namespace WishList.Domain.UseCases.AddWishList;

public class SuggestAddingUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage,
    IMapper mapper)
    : ISuggestAddingUseCase
{
    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        const string textMessage = "Добавить первый WishList?";
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Давай скорее!", $"{Way.CreateWishList}/{StepWay.First}"),
                }
            }); 
        await sender.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: textMessage,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}