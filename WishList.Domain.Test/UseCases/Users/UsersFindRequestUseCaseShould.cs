using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Domain.UseCases.Users.UsersFindRequest;

namespace WishList.Domain.Test.UseCases.Users;

public class UsersFindRequestUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly UsersFindRequestUseCase sut;

    public UsersFindRequestUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new UsersFindRequestUseCase(sender.Object, new Mock<IUpdateUserUseCase>().Object);
    }
    
    [Fact]
    public async Task AnswerCallbackQueryAndSendMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new UsersFindRequestCommand(param);
        
        await sut.Handle(request, CancellationToken.None);

        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}