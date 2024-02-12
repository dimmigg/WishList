using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.Start;

public class StartUseCase(
    ITelegramBotClient botClient,
    IUpdateUserUseCase updateUserUseCase) : UseCaseBase, IStartUseCase
{
    public async Task<Message> Execute(Message message, CancellationToken cancellationToken)
    {
        if (message.From != null)
        {
            await updateUserUseCase.CreateOrUpdateUser(message.From, cancellationToken);
        }

        const string usage = "О, привет!\n" +
                             "Я помогу узнать, что хотят получить твои друзья! А также рассказать им, что хочешь получить ты!\n\n" +
                             "Для поиска друзей просто отправь ник.\n" + 
                             "А для добавления своих пожеланий воспользуйся клавиатурой.";
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { "Мои списки", "1.2" },
                new KeyboardButton[] { "2.1", "2.2" },
            })
        {
            ResizeKeyboard = true
        };

        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
