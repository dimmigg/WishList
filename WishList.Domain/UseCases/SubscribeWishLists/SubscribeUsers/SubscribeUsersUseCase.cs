using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeUsers;

public class SubscribeUsersUseCase(
    ITelegramSender telegramSender)
    : IRequestHandler<SubscribeUsersCommand>
{
    public async Task Handle(SubscribeUsersCommand request, CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard = [];

        var users = request.Param.User.SubscribeWishLists.GroupBy(swl => swl.Author).ToArray();
        var sb = new StringBuilder();
        if (users.Length != 0)
        {
            sb.AppendLine("Список друзей:");
            keyboard = users
                .Select(gr => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{gr.Key} ({gr.Count()})",
                        $"{Commands.SUBSCRIBE_USER_WISH_LISTS}<?>{gr.Key.Id}"),
                }).ToList();
        }
        else
        {
            sb.AppendLine("Еще нет подписок");
        }

        keyboard.Add(
        [
            InlineKeyboardButton.WithCallbackData(
                "🔍 Поиск", Commands.USERS_FIND_REQUEST)
        ]);
        keyboard = keyboard.AddBaseFooter();

        await telegramSender.EditMessageAsync(
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}