using Moq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.SuggestAddWishList;

namespace WishList.Domain.Test.UseCases;

public class SuggestAddWishListShould
{
    private readonly SuggestAddWishListUseCase sut;
    private readonly Mock<ISender> sender;

    public SuggestAddWishListShould()
    {
        sender = new Mock<ISender>();
        sut = new SuggestAddWishListUseCase(sender.Object);
    }

    [Fact]
    public async Task SuggestAddWishList_WhenNotWishListInDd()
    {
        const long userId = 123;

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
            It.IsAny<int?>(),
            It.IsAny<ParseMode?>(),
            It.IsAny<IEnumerable<MessageEntity>?>(),
            It.IsAny<bool?>(),
            It.IsAny<bool?>(),
            It.IsAny<bool?>(),
            It.IsAny<int?>(),
            It.IsAny<bool?>(),
            It.IsAny<IReplyMarkup?>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}