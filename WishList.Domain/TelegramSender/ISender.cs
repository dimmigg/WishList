using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace WishList.Domain.TelegramSender;

public interface ISender
{
    Task<Message> EditMessageTextAsync(
        ChatId chatId,
        int messageId,
        string text,
        ParseMode? parseMode = ParseMode.Markdown,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = true,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default);

    Task<Message> SendTextMessageAsync(
        ChatId chatId,
        string text,
        int? messageThreadId = default,
        ParseMode? parseMode = ParseMode.Markdown,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default);

    Task AnswerInlineQueryAsync(
        string inlineQueryId,
        IEnumerable<InlineQueryResult> results,
        int? cacheTime = default,
        bool? isPersonal = default,
        string? nextOffset = default,
        InlineQueryResultsButton? button = default,
        CancellationToken cancellationToken = default
    );

    Task AnswerCallbackQueryAsync(
        string callbackQueryId,
        string? text = default,
        bool? showAlert = default,
        string? url = default,
        int? cacheTime = default,
        CancellationToken cancellationToken = default);

    Task DeleteMessageAsync(
        ChatId chatId,
        int messageId,
        CancellationToken cancellationToken = default);

}