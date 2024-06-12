using FluentAssertions;
using Moq;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyWishLists.MyWishListEditName;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases.MyWishLists;

public class MyWishListEditNameUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly MyWishListEditNameUseCase sut;
    private readonly Mock<IWishListStorage> wishListStorage;

    public MyWishListEditNameUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        wishListStorage = new Mock<IWishListStorage>();
        sut = new MyWishListEditNameUseCase(sender.Object, wishListStorage.Object, new Mock<IUpdateUserUseCase>().Object);
    }
    
    [Fact]
    public async Task EditNameWishListAndSendMessage_WhenValidParams()
    {
        var param = GetMessageParamValid();
        const int wishListId = 1;
        param.Command = $"commad<?>{wishListId}";
        param.Message!.Text = "WishList";
        var request = new MyWishListEditNameCommand(param);
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        
        wishListStorage.Verify(s => s.EditName(
                "WishList",
                wishListId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task ThrowDomainException_WhenIncompleteParams()
    {
        var param = GetMessageParamValid();
        param.Command = $"command";
        var request = new MyWishListEditNameCommand(param);
        
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
        var param = GetMessageParamValid();
        param.Message!.Text = "WishList";
        const string commandParams = "InvalidParams";
        param.Command = $"command<?>{commandParams}";
        var request = new MyWishListEditNameCommand(param);
        
        await sut.Invoking(s => s.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
        
        wishListStorage.Verify(s => s.EditName(
                "WishList",
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
}