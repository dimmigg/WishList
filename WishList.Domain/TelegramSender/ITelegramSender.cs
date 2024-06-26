﻿using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WishList.Domain.TelegramSender;

public interface ITelegramSender
{
    int? MessageId { get; set; }
    ChatId? ChatId { get; set; }
    string? InlineQueryId { get; set; }
    string? CallbackQueryId { get; set; }
    
    Task<Message> EditMessageAsync(
        string text,
        ChatId chatId,
        int messageId,
        InlineKeyboardMarkup? replyMarkup = default,
        ParseMode? parseMode = ParseMode.MarkdownV2,
        bool? disableWebPagePreview = true,
        CancellationToken cancellationToken = default);
    
    Task<Message> EditMessageAsync(
        string text,
        InlineKeyboardMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default);

    Task<Message> SendMessageAsync(
        string text,
        IReplyMarkup? replyMarkup = default,
        ChatId? chatId = default,
        int? messageThreadId = default,
        ParseMode? parseMode = ParseMode.MarkdownV2,
        IEnumerable<MessageEntity>? entities = default,
        bool? disableWebPagePreview = default,
        bool? disableNotification = true,
        bool? protectContent = default,
        int? replyToMessageId = default,
        bool? allowSendingWithoutReply = default,
        CancellationToken cancellationToken = default);
    
    Task<Message> SendMessageAsync(
        string text,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default);
    
    Task AnswerCallbackQueryAsync(
        string? text = null,
        bool? showAlert = false,
        CancellationToken cancellationToken = default);

    Task DeleteMessageAsync(
        ChatId? chatId = default,
        int? messageId = default,
        CancellationToken cancellationToken = default);
    
    Task ShowAlertAsync(string message, CancellationToken cancellationToken = default);
    
    Task SendChatActionAsync(ChatAction action, CancellationToken cancellationToken);   

}