using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListParams;
using WishList.Storage.Entities;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListParamsUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListParamsUseCase sut;
    private readonly ISetup<IWishListStorage,Task<Storage.Entities.WishList?>> wlStorageGetWishListSetup;

    public MyWishListParamsUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var wishListStorage = new Mock<IWishListStorage>();
        wlStorageGetWishListSetup = wishListStorage.Setup(wl => wl.GetWishList(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        sut = new MyWishListParamsUseCase(sender.Object, wishListStorage.Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListParamsCommand(param);
        var wishList = new Storage.Entities.WishList()
        {
            Id = 1,
            Name = "wishList",
            Presents = new List<Present>(),
            IsPrivate = true,
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
    public async Task ThrowDomainException_WhenWishListNotFound()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new MyWishListParamsCommand(param);

        wlStorageGetWishListSetup.ReturnsAsync((Storage.Entities.WishList)null!);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task ThrowDomainException_WhenIncompleteParams()
    {
        var param = GetCallbackQueryParamValid();
        param.Command = "command";
        var request = new MyWishListParamsCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        sender.Verify(s => s.SendMessageAsync(
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
        var request = new MyWishListParamsCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}