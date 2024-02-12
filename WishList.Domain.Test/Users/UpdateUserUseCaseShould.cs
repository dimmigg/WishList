using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Test.Users;

public class UpdateUserUseCaseShould
{
    private readonly ISetup<IGetUserStorage, Task<User?>> getUserSetup;
    private readonly ISetup<IAddUserStorage,Task<User>> addUserSetup;
    private readonly ISetup<IUpdateUserStorage,Task> updateUserSetup;
    private readonly UpdateUserUseCase sut;
    private readonly Mock<IGetUserStorage> getUserStorage;
    private readonly Mock<IAddUserStorage> addUserStorage;
    private readonly Mock<IUpdateUserStorage> updateUserStorage;

    public UpdateUserUseCaseShould()
    {
        getUserStorage = new Mock<IGetUserStorage>();
        getUserSetup = getUserStorage
            .Setup(s => s.GetUser(It.IsAny<long>(), It.IsAny<CancellationToken>()));
        
        addUserStorage = new Mock<IAddUserStorage>();
        addUserSetup = addUserStorage
            .Setup<Task<User>>(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        updateUserStorage = new Mock<IUpdateUserStorage>();
        updateUserSetup = updateUserStorage
            .Setup(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()));
        
        sut = new UpdateUserUseCase(getUserStorage.Object, addUserStorage.Object, updateUserStorage.Object);
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
        updateUserStorage.Verify(u => u.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        addUserStorage.Verify(u => u.AddUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task AddUser_WhenUserInDb()
    {
        const long userId = 123;
        var user = new User
        {
            Id = userId
        };
        
        getUserSetup.ReturnsAsync(user);
        addUserSetup.ReturnsAsync(new User
        {
            Id = userId
        });
        
        await sut.CreateOrUpdateUser(user, CancellationToken.None);
        addUserStorage.Verify(u => u.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        updateUserStorage.Verify(u => u.UpdateUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    
}