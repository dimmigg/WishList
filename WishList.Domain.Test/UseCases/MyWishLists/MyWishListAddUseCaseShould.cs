using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListAdd;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListAddUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListAddUseCase sut;
    public MyWishListAddUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new MyWishListAddUseCase(
            sender.Object, 
            new Mock<IWishListStorage>().Object, 
            new Mock<IUpdateUserUseCase>().Object);
    }
    
    [Fact]
    public async Task SendMessage_WhenValidParams()
    {
        var param = GetMessageParamValid();
        var request = new MyWishListAddCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}