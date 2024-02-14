using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace WishList.Domain.TelegramSender;

public class Sender(ITelegramBotClient botClient) : ISender
{
    public Task<Message> EditMessageTextAsync(ChatId chatId, int messageId, string text, ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default, bool? disableWebPagePreview = default, InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default) => 
        botClient.EditMessageTextAsync(
            chatId,
            messageId,
            text,
            parseMode,
            entities,
            disableWebPagePreview,
            replyMarkup,
            cancellationToken);

    public Task<Message> SendTextMessageAsync(ChatId chatId, string text, int? messageThreadId = default, ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default, bool? disableWebPagePreview = default, bool? disableNotification = default,
        bool? protectContent = default, int? replyToMessageId = default, bool? allowSendingWithoutReply = default,
        IReplyMarkup? replyMarkup = default, CancellationToken cancellationToken = default) =>
        botClient.SendTextMessageAsync(
            chatId,
            text,
            messageThreadId,
            parseMode,
            entities,
            disableWebPagePreview,
            disableNotification,
            protectContent,
            replyToMessageId,
            allowSendingWithoutReply,
            replyMarkup,
            cancellationToken);

    public Task AnswerInlineQueryAsync(string inlineQueryId, IEnumerable<InlineQueryResult> results, int? cacheTime = default,
        bool? isPersonal = default, string? nextOffset = default, InlineQueryResultsButton? button = default,
        CancellationToken cancellationToken = default) => 
        botClient.AnswerInlineQueryAsync(
            inlineQueryId,
            results,
            cacheTime,
            isPersonal,
            nextOffset,
            button,
            cancellationToken);
}