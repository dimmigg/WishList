using Moq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Main.Main;

namespace WishList.Domain.Test.UseCases.Main;

public class MainUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MainUseCase sut;

    public MainUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new MainUseCase(sender.Object);
    }

    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MainCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<ChatId?>(),
                It.IsAny<int?>(),
                It.IsAny<ParseMode?>(),
                It.IsAny<IEnumerable<MessageEntity>?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendMessage_WhenValidParams()
    {
        var param = GetMessageParamValid();
        var request = new MainCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task NotSendOrChangeMessage_WhenInvalidParam()
    {
        var param = GetMessageParamValid();
        param.Message = null;
        var request = new MainCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}