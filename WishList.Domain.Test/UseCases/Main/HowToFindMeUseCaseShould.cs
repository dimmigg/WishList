using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Main.HowToFindMe;

namespace WishList.Domain.Test.UseCases.Main;

public class HowToFindMeUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly HowToFindMeUseCase sut;

    public HowToFindMeUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new HowToFindMeUseCase(sender.Object);
    }

    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new HowToFindMeCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}