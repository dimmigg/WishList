using FluentAssertions;
using Moq;
using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.CommandOptions;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.Received.CallbackQueryReceived;

public class CallbackQueryBuilderShould
{
    private readonly CallbackQueryBuilder sut;

    public CallbackQueryBuilderShould()
    {
        sut = new CallbackQueryBuilder(
            new Mock<IUserStorage>().Object,
            new Mock<IWishListStorage>().Object,
            new Mock<ISender>().Object
            );
    }   

    [Fact]
    public void ReturnCreateWishList_WhenWayCreateWishList()
    {
        var received = sut.Build(Command.CreateWishList, CommandStep.Null, CancellationToken.None);

        received.Should().BeOfType<CreateWishListCallbackReceived>();
    }
    
    [Fact]
    public void ThrowDomain_WhenWayInvalid()
    {
        sut.Invoking(s => s.Build(Command.Null, CommandStep.Null, CancellationToken.None))
            .Should().Throw<DomainException>();
    }
}