using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListDelete;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListDeleteUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListDeleteUseCase sut;
    private readonly ISetup<IWishListStorage,Task<Storage.Entities.WishList?>> wlStorageGetWishListSetup;

    public MyWishListDeleteUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var wishListStorage = new Mock<IWishListStorage>();
        wlStorageGetWishListSetup = wishListStorage.Setup(wl => wl.GetWishList(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        sut = new MyWishListDeleteUseCase(sender.Object, wishListStorage.Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListDeleteCommand(param);
        var wishList = new Storage.Entities.WishList()
        {
            Name = "wishList"
        };
        wlStorageGetWishListSetup.ReturnsAsync(wishList);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task ShowAlert_WhenInvalidParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListDeleteCommand(param);
        var wishList = new Storage.Entities.WishList()
        {
            Name = "wishList"
        };
        wlStorageGetWishListSetup.ReturnsAsync(wishList);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        sender.Verify(s => s.ShowAlertAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task ShowAlert_WhenWishListNotFound()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListDeleteCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        sender.Verify(s => s.ShowAlertAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
    }
}