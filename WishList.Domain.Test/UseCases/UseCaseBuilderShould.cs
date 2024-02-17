using FluentAssertions;
using Moq;
using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases;
using WishList.Domain.UseCases.Builder;
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
            new Mock<ISender>().Object);
    }

    [Fact]
    public void ReturnStartUseCase_WhenValidCommand()
    {
        param.Command = "main";
        sut
            .Build(param)
            .Should()
            .BeOfType<StartUseCase>();
    }
    
    [Fact]
    public void ReturnGetWishListUseCase_WhenValidCommand()
    {
        param.Command = "my-wish-lists";
        sut
            .Build(param)
            .Should()
            .BeOfType<GetWishListsUseCase>();
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