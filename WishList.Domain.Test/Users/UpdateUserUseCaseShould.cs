using AutoMapper;
using Moq;
using Telegram.Bot.Types;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Test.Users;

public class UpdateUserUseCaseShould
{
    private readonly UpdateUserUseCase sut;
    private readonly Mock<IUserStorage> userStorage;

    public UpdateUserUseCaseShould()
    {
        userStorage = new Mock<IUserStorage>();
        userStorage
            .Setup(s => s.GetUser(It.IsAny<long>(), It.IsAny<CancellationToken>()));
        userStorage
            .Setup<Task<TelegramUser>>(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));
        userStorage
            .Setup<Task<TelegramUser>>(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));
        
        sut = new UpdateUserUseCase(userStorage.Object, new Mock<IMapper>().Object);
    }

    [Fact]
    public async Task AddUser_WhenNotUserInDb()
    {
        const long userId = 123;
        var user = new User
        {
            Id = userId
        };
        
        await sut.CreateOrUpdateUser(user, CancellationToken.None);
        userStorage.Verify(u => u.UpdateUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddUser_WhenUserInDb()
    {
        const long userId = 123;
        var user = new User
        {
            Id = userId
        };

        await sut.CreateOrUpdateUser(user, CancellationToken.None);
        userStorage.Verify(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        userStorage.Verify(u => u.UpdateUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}