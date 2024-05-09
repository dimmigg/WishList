using Telegram.Bot.Types;
using WishList.Domain.Constants;
using WishList.Domain.Models;

namespace WishList.Domain.Test.UseCases;

public class UseCaseBase
{
    private const long CHAT_ID = 1;
    private const int MY_MESSAGE_ID = 1;
    private const long MY_USER_ID = 1;
    private const string MY_CALLBACK_ID = "1";
    
    internal static UseCaseParam GetMessageParamValid()
    {
        var msg = new Message
        {
            MessageId = MY_MESSAGE_ID,
            Chat = new Chat
            {
                Id = CHAT_ID
            }
        };
        var user = new RegisteredUser
        {
            Id = MY_USER_ID
        };
        return new UseCaseParam
        {
            Command = Commands.MAIN,
            User = user,
            Message = msg,
        };
    }
    
    internal static UseCaseParam GetCallbackQueryParamValid()
    {
        var msg = new Message
        {
            MessageId = MY_MESSAGE_ID,
            Chat = new Chat
            {
                Id = CHAT_ID
            }
        };
        var cq = new CallbackQuery
        {
            Id = MY_CALLBACK_ID,
            Message = msg,
            From = new User
            {
                Id = MY_USER_ID
            }
        };
        var user = new RegisteredUser
        {
            Id = MY_USER_ID
        };
        return new UseCaseParam
        {
            Command = Commands.MAIN,
            User = user,
            CallbackQuery = cq,
        };
    }
}