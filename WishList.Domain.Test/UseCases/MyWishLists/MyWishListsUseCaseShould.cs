using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishLists;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListsUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListsUseCase sut;
    public MyWishListsUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        sut = new MyWishListsUseCase(sender.Object, new Mock<IWishListStorage>().Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListsCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}