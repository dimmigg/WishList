using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListAddRequestUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListAddRequestUseCase sut;
    public MyWishListAddRequestUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new MyWishListAddRequestUseCase(
            sender.Object, 
            new Mock<IUpdateUserUseCase>().Object);
    }
    
    [Fact]
    public async Task SendMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListAddRequestCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}