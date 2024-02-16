﻿using Telegram.Bot.Types;

namespace WishList.Domain.Received.CallbackQueryReceived;

public interface ICallbackReceived
{
    Task Execute(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    Task Execute(Message message, CancellationToken cancellationToken);
}