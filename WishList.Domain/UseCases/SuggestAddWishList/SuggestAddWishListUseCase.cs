using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.WayOptions;

namespace WishList.Domain.UseCases.SuggestAddWishList;

public class SuggestAddWishListUseCase(
    ISender sender)
    : ISuggestAddWishListUseCase
{
    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        const string textMessage = "Добавить новый WishList?";
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
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