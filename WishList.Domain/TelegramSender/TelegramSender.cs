using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;

namespace WishList.Domain.TelegramSender;

public class TelegramSender(ITelegramBotClient botClient) : ITelegramSender
{
    public int? MessageId { get; set; }
    public ChatId? ChatId { get; set; }
    public string? InlineQueryId { get; set; }
    public string? CallbackQueryId { get; set; }

    public async Task<Message> EditMessageAsync(
        string text,
        InlineKeyboardMarkup? replyMarkup = default,
        ChatId? chatId = default,
        int? messageId = default,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        CancellationToken cancellationToken = default)
    {
        if ((chatId is not null || ChatId is not null) &&
            (messageId.HasValue || MessageId.HasValue))
            return await botClient.EditMessageTextAsync(
                chatId ?? ChatId!,
                messageId ?? MessageId!.Value,
                text,
                parseMode,
                entities,
                disableWebPagePreview,
                replyMarkup,
                cancellationToken);
        else
            throw new DomainException("Chat or Message not found");
    }

    public async Task<Message> SendMessageAsync(
        string text,
        IReplyMarkup? replyMarkup = default,
        ChatId? chatId = default,
        int? messageThreadId = default,
        ParseMode? parseMode = default,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = default,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        CancellationToken cancellationToken = default)
    {
        if (chatId is not null || ChatId is not null)
            return await botClient.SendTextMessageAsync(
                chatId ?? ChatId!,
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
        else
            throw new DomainException("Chat not found");
    }

    public Task AnswerInlineQueryAsync(string inlineQueryId, IEnumerable<InlineQueryResult> results,
        int? cacheTime = default,
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

    public async Task AnswerCallbackQueryAsync(
        string? text = default,
        string? callbackQueryId = default,
        bool? showAlert = default,
        string? url = default,
        int? cacheTime = default,
        CancellationToken cancellationToken = default)
    {
        if (callbackQueryId is not null || CallbackQueryId is not null)
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId ?? CallbackQueryId!,
                text,
                showAlert,
                url,
                cacheTime,
                cancellationToken);
    }

    public async Task DeleteMessageAsync(
        ChatId? chatId = default,
        int? messageId = default,
        CancellationToken cancellationToken = default)
    {
        if ((chatId is not null || ChatId is not null) &&
            (messageId.HasValue || MessageId.HasValue))
            await botClient.DeleteMessageAsync(
                chatId ?? ChatId!,
                messageId ?? MessageId!.Value,
                cancellationToken);
    }
}