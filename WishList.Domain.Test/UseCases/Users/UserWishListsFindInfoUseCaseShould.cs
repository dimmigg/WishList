using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Users.UserWishListsFindInfo;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.Users;

public class UserWishListsFindInfoUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly UserWishListsFindInfoUseCase sut;
    private readonly ISetup<IWishListStorage,Task<Storage.Entities.WishList[]>> wlStorageGetWishListSetup;

    public UserWishListsFindInfoUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var wishListStorage = new Mock<IWishListStorage>();
        wlStorageGetWishListSetup = wishListStorage.Setup(wl => wl.GetWishLists(It.IsAny<long>(), It.IsAny<CancellationToken>()));
        sut = new UserWishListsFindInfoUseCase(sender.Object, wishListStorage.Object, new Mock<IUserStorage>().Object);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParamsAndWishLists()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new UserWishListsFindInfoCommand(param);
        var wishList = new Storage.Entities.WishList()
        {
            Id = 1,
            Name = "wishList",
            Presents = new List<Present>(),
            IsPrivate = true,
        };
        wlStorageGetWishListSetup.ReturnsAsync([wishList]);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.EditMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParamsAndEmptyWishLists()
    {
        var param = GetCallbackQueryParamValid();
        const int wishListId = 1;
        param.Command = $"command<?>{wishListId}";
        var request = new UserWishListsFindInfoCommand(param);
        wlStorageGetWishListSetup.ReturnsAsync([]);
        
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
        param.Command = "command";
        var request = new UserWishListsFindInfoCommand(param);
        
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
        var request = new UserWishListsFindInfoCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}