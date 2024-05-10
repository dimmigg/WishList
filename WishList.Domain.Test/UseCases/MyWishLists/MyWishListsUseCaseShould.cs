using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishLists;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListsUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListsUseCase sut;
    private readonly ISetup<IWishListStorage,Task<Storage.Entities.WishList[]>> getWishListsSetup;

    public MyWishListsUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var wishListStorage = new Mock<IWishListStorage>();
        getWishListsSetup = wishListStorage
            .Setup(wl => wl.GetWishLists(It.IsAny<long>(), It.IsAny<CancellationToken>()));
        sut = new MyWishListsUseCase(sender.Object, wishListStorage.Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParamsAndEmptyList()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListsCommand(param);
        getWishListsSetup.ReturnsAsync([]);
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParamsAndNotEmptyList()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListsCommand(param);

        var wl = new Storage.Entities.WishList
        {
            Id = 0,
            Name = "wl",
            AuthorId = 0,
            IsPrivate = false,
            Presents = []
        };

        getWishListsSetup.ReturnsAsync([wl]);
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}