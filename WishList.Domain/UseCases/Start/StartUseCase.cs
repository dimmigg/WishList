using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.SuggestAddWishList;
namespace WishList.Domain.UseCases.Start;

public class StartUseCase(
    ISender sender,
    ISuggestAddWishListUseCase suggestAddWishListWishListUseCase) : UseCaseBase, IStartUseCase
{
    public async Task Execute(Message message, RegisteredUser user, CancellationToken cancellationToken)
    {
        const string usage = "О, привет!\n" +
                             "Я помогу узнать, что хотят получить твои друзья! А также рассказать им, что хочешь получить ты!\n\n" +
                             "Для поиска друзей просто отправь ник.\n" + 
                             "А для добавления своих пожеланий воспользуйся клавиатурой.";
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "Мои списки", "Параметры" },
            })
        {
            ResizeKeyboard = true
        };

        await sender.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
        
        if (user.WishLists.Count == 0)
        {
            //новый юзер - предлагаем добавить список
            await suggestAddWishListWishListUseCase.Execute(message, cancellationToken);
        }
        else
        {
            // у пользователя есть список...
        }
    }
}
