using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Users.UsersFindRequest;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Test.UseCases.Users;

public class UsersFindRequestUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly UsersFindRequestUseCase sut;
    private readonly Mock<IUserStorage> userStorage;

    public UsersFindRequestUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        userStorage = new Mock<IUserStorage>();
        sut = new UsersFindRequestUseCase(sender.Object, userStorage.Object);
    }
    
    [Fact]
    public async Task AnswerCallbackQueryAndSendMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new UsersFindRequestCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        userStorage.Verify(u => u.UpdateLastCommandUser(
            It.IsAny<long>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}