using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Test.Users;

public class UpdateUserUseCaseShould
{
    private readonly UpdateUserUseCase sut;
    private readonly Mock<IUserStorage> userStorage;
    private readonly ISetup<IUserStorage,Task<User?>> getUserSetup;
    private readonly ISetup<IUserStorage,Task<User>> addUserSetup;
    private readonly ISetup<IUserStorage,Task> updateUserSetup;

    public UpdateUserUseCaseShould()
    {
        userStorage = new Mock<IUserStorage>();
        getUserSetup = userStorage
            .Setup(s => s.GetUser(It.IsAny<long>(), It.IsAny<CancellationToken>()));
        
        addUserSetup = userStorage
            .Setup<Task<User>>(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));
        
        updateUserSetup = userStorage
            .Setup(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));
        
        sut = new UpdateUserUseCase(userStorage.Object);
    }

    [Fact]
    public async Task AddUser_WhenNotUserInDb()
    {
        const long userId = 123;
        getUserSetup.ReturnsAsync((User?)null);
        addUserSetup.ReturnsAsync(new User
        {
            Id = userId
        });

        var user = new User
        {
            Id = userId
        };
        
        await sut.CreateOrUpdateUser(user, CancellationToken.None);
        userStorage.Verify(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        userStorage.Verify(u => u.AddUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddUser_WhenUserInDb()
    {
        const long userId = 123;
        var user = new User
        {
            Id = userId
        };

        getUserSetup.ReturnsAsync(new User
        {
            Id = userId
        });
        addUserSetup.ReturnsAsync(new User
        {
            Id = userId
        });

        await sut.CreateOrUpdateUser(user, CancellationToken.None);
        userStorage.Verify(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        userStorage.Verify(u => u.UpdateUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }
}