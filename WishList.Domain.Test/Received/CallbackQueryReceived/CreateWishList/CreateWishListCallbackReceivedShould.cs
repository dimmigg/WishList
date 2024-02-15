using Moq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.WayOptions;

namespace WishList.Domain.Test.Received.CallbackQueryReceived.CreateWishList;

public class CreateWishListCallbackReceivedShould
{
    private readonly CreateWishListCallbackReceived sut;
    private readonly Mock<IUserStorage> userStorage;
    private readonly Mock<ISender> sender;

    public CreateWishListCallbackReceivedShould()
    {
        userStorage = new Mock<IUserStorage>();
        sender = new Mock<ISender>();
        sut = new CreateWishListCallbackReceived(
            Way.Null,
            StepWay.Null,
            userStorage.Object,
            sender.Object
            );
    }

    [Fact]
    public async Task SendMessage_WhenWayNotNull()
    {
        const Way way = Way.CreateWishList;
        const StepWay stepWay = StepWay.First;
        const long userId = 1;
        const string callbackId = "1";
        const long chatId = 1;
        sut.SetWay(way);
        sut.SetStepWay(stepWay);

        var callbackQuery = new CallbackQuery
        {
            Id = callbackId,
            Message = new Message
            {
                Chat = new Chat
                {
                    Id = chatId
                }
            },
            From = new User
            {
                Id = userId
            }
        };
        await sut.Execute(callbackQuery, CancellationToken.None);
        
        userStorage.Verify(us => us.UpdateWayUser(
            userId, 
            way, 
            stepWay, 
            It.IsAny<CancellationToken>()),
            Times.Once);
        sender.Verify(s => s.SendTextMessageAsync(
            chatId,
            It.IsAny<string>(),
            It.IsAny<int?>(),
            It.IsAny<ParseMode?>(),
            It.IsAny<IEnumerable<MessageEntity>?>(),
            It.IsAny<bool?>(),
            It.IsAny<bool?>(),
            It.IsAny<bool?>(),
            It.IsAny<int?>(),
            It.IsAny<bool?>(),
            It.IsAny<IReplyMarkup?>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
        sender.Verify(s => s.AnswerCallbackQueryAsync(
            callbackId,
            It.IsAny<string>(),
            It.IsAny<bool?>(),
            It.IsAny<string>(),
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}