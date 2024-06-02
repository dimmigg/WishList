using Moq;
using Moq.Language.Flow;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Users.UsersFind;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Test.UseCases.Users;

public class UsersFindUseCaseShould : UseCaseBase
{
    private readonly Mock<ITelegramSender> sender;
    private readonly UsersFindUseCase sut;
    private readonly ISetup<IUserStorage,Task<TelegramUser[]?>> findUsersSetup;

    public UsersFindUseCaseShould()
    {
        sender = new Mock<ITelegramSender>();
        var userStorage = new Mock<IUserStorage>();
        findUsersSetup = userStorage.Setup(u => u.FindUsers(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        sut = new UsersFindUseCase(
            sender.Object, 
            userStorage.Object);
    }
    
    [Fact]
    public async Task SendMessage_WhenValidParamsAndSubscribeUsers()
    {
        var param = GetMessageParamValid();
        param.Message!.Text = "FindUser";
        findUsersSetup.ReturnsAsync([
            new TelegramUser
            {
                Id = 0,
                FirstName = "FirstName",
                LastName = "LastName",
                Username = "Username",
                LastCommand = null,
                WishLists = new List<Storage.Entities.WishList>(),
                SubscribeWishLists = new List<Storage.Entities.WishList>(),
                WriteWishLists = new List<Storage.Entities.WishList>()
            }
        ]);
        
        var request = new UsersFindCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendMessage_WhenValidParamsAndNotSubscribeUsers()
    {
        var param = GetMessageParamValid();
        param.Message!.Text = "FindUser";
        findUsersSetup.ReturnsAsync([]);
        var request = new UsersFindCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }    
    
    [Fact]
    public async Task SendMessage_WhenValidParamsAndNotValidMessageAndNotSubscribeUsers()
    {
        var param = GetMessageParamValid();
        param.Message!.Text = "User";
        findUsersSetup.ReturnsAsync([]);
        var request = new UsersFindCommand(param);
        
        await sut.Handle(request, CancellationToken.None);
        
        sender.Verify(s => s.SendMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReplyMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}