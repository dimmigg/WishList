using AutoMapper;
using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.AddWishList;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using WishList.Storage.WayOptions;

namespace WishList.Domain.Test.UseCases;

public class AddWishListShould
{
    private readonly ISetup<IUserStorage,Task<TelegramUser>> updateUserSetup;
    private readonly SuggestAddingUseCase sut;
    private readonly Mock<ISender> sender;

    public AddWishListShould()
    {
        var wishListStorage = new Mock<IWishListStorage>();
        wishListStorage
            .Setup(wl => wl.AddWishList(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()));
        var userStorage = new Mock<IUserStorage>();
        updateUserSetup = userStorage
            .Setup<Task<TelegramUser>>(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));


        sender = new Mock<ISender>();
        sut = new SuggestAddingUseCase(
            sender.Object,
            wishListStorage.Object,
            userStorage.Object,
            new Mock<IMapper>().Object
            );
    }

    //[Fact]
    public async Task AddWishList_WhenNotWishListInDd()
    {
        const long userId = 123;
        var tgUser = new TelegramUser
        {
            Id = userId,
            CurrentWay = Way.CreateWishList,
            WayStep = StepWay.Null,
            WishLists = new List<Storage.Entities.WishList>()
        };
        updateUserSetup.ReturnsAsync(tgUser);

        var user = new User
        {
            Id = userId
        };

        var message = new Message()
        {
            From = user,
            Chat = new Chat
            {
                Id = 0
            } 
        };
        
        await sut.Execute(message, CancellationToken.None);
        
        sender.Verify(u => u.SendTextMessageAsync(
            It.IsAny<ChatId>(),
            It.IsAny<string>(),
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}