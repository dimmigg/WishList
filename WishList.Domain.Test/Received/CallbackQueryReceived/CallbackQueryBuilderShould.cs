using FluentAssertions;
using Moq;
using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.WayOptions;

namespace WishList.Domain.Test.Received.CallbackQueryReceived;

public class CallbackQueryBuilderShould
{
    private readonly CallbackQueryBuilder sut;

    public CallbackQueryBuilderShould()
    {
        sut = new CallbackQueryBuilder(
            new Mock<IUserStorage>().Object,
            new Mock<ISender>().Object
            );
    }   

    [Fact]
    public void ReturnCreateWishList_WhenWayCreateWishList()
    {
        var command = $"{Way.CreateWishList}/{StepWay.Null}";

        var received = sut.Build(command, CancellationToken.None);

        received.Should().BeOfType<CreateWishListCallbackReceived>();
    }
    
    [Fact]
    public void ThrowDomain_WhenWayInvalid()
    {
        const string command = "InvalidWay/StepWayNull";

        sut.Invoking(s => s.Build(command, CancellationToken.None))
            .Should().Throw<DomainException>();
    }
    
    [Fact]
    public void ThrowDomain_WhenWayNull()
    {
        var command = $"{Way.Null}/{StepWay.Null}";

        sut.Invoking(s => s.Build(command, CancellationToken.None))
            .Should().Throw<DomainException>();
    }
}