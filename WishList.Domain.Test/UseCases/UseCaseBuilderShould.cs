using FluentAssertions;
using WishList.Domain.Constants;
using WishList.Domain.Models;
using WishList.Domain.UseCases.Builder;
using WishList.Domain.UseCases.Main.Main;
using WishList.Domain.UseCases.MyWishLists.MyWishLists;

namespace WishList.Domain.Test.UseCases;

public class UseCaseBuilderShould
{
    private readonly UseCaseBuilder sut;
    private readonly UseCaseParam param;

    public UseCaseBuilderShould()
    {
        param = new UseCaseParam();
        sut = new UseCaseBuilder();
    }

    [Fact]
    public void ReturnStartUseCase_WhenValidCommand()
    {
        param.Command = Commands.Main;
        sut
            .Build(param)
            .Should()
            .BeOfType<MainCommand>();
    }
    
    [Fact]
    public void ReturnGetWishListUseCase_WhenValidCommand()
    {
        param.Command = Commands.WishLists;
        sut
            .Build(param)
            .Should()
            .BeOfType<MyWishListsCommand>();
    }    
}