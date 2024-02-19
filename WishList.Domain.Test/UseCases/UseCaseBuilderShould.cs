using FluentAssertions;
using Moq;
using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases;
using WishList.Domain.UseCases.Builder;
using WishList.Domain.UseCases.MyWishLists;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Test.UseCases;

public class UseCaseBuilderShould
{
    private readonly UseCaseBuilder sut;
    private readonly UseCaseParam param;

    public UseCaseBuilderShould()
    {
        param = new UseCaseParam();
        sut = new UseCaseBuilder(
            new Mock<IWishListStorage>().Object,
            new Mock<IUserStorage>().Object,
            new Mock<IPresentStorage>().Object,
            new Mock<ISender>().Object);
    }

    [Fact]
    public void ReturnStartUseCase_WhenValidCommand()
    {
        param.Command = "main";
        sut
            .Build(param)
            .Should()
            .BeOfType<MainUseCase>();
    }
    
    [Fact]
    public void ReturnGetWishListUseCase_WhenValidCommand()
    {
        param.Command = "my-wish-lists";
        sut
            .Build(param)
            .Should()
            .BeOfType<MyWishListsUseCase>();
    }    
    
    [Fact]
    public void ThrowDomainException_WhenInvalidCommand()
    {
        param.Command = "invalid";
        sut.Invoking(s => s.Build(param))
            .Should()
            .Throw<DomainException>();
    }
}