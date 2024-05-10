using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListDeleteRequest;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListDeleteRequestUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListDeleteRequestUseCase sut;
    private readonly ISetup<IWishListStorage,Task<Storage.Entities.WishList?>> wlStorageGetWishListSetup;

    public MyWishListDeleteRequestUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var wishListStorage = new Mock<IWishListStorage>();
        wlStorageGetWishListSetup = wishListStorage.Setup(wl => wl.GetWishList(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        sut = new MyWishListDeleteRequestUseCase(sender.Object, wishListStorage.Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListDeleteRequestCommand(param);
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
    public async Task ThrowDomainException_WhenIncompleteParams()
    {
        var param = GetCallbackQueryParamValid();
        var request = new MyWishListDeleteRequestCommand(param);
        var wishList = new Storage.Entities.WishList()
        {
            Name = "wishList"
        };
        wlStorageGetWishListSetup.ReturnsAsync(wishList);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ThrowDomainException_WhenWishListNotFound()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListDeleteRequestCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ThrowDomainException_WhenInvalidParams()
    {
        var param = GetCallbackQueryParamValid();
        const string commandParams = "InvalidParams";
        param.Command = $"command<?>{commandParams}";
        var request = new MyWishListDeleteRequestCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}