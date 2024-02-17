using FluentAssertions;
using Moq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases;

namespace WishList.Domain.Test.UseCases;

public class GetWishListsUseCaseShould
{
    private long chatId = 1;
    private int messageId = 1;
    private long userId = 1;
    private string callbackId = "1";
    private readonly Mock<ISender> sender;
    private readonly UseCaseParam param;
    private readonly StartUseCase sut;
    public GetWishListsUseCaseShould()
    {
        param = new UseCaseParam();
        sender = new Mock<ISender>();
        sut = new StartUseCase(param, sender.Object);
    }
    
    private void SetValidParamSetups()
    {
        var msg = new Message
        {
            MessageId = messageId,
            Chat = new Chat
            {
                Id = chatId
            }
            
        };
        var cq = new CallbackQuery
        {
            Id = callbackId,
            Message = msg,
            From = new User
            {
                Id = userId
            }
        };
        param.Message = msg;
        param.CallbackQuery = cq;
    }
    
    [Fact]
    public async Task EditMessage_WhenValidParams()
    {
        SetValidParamSetups();
        param.Message = null;
        await sut.Execute(CancellationToken.None);
        sender.Verify(s => s.EditMessageTextAsync(
                chatId,
                messageId,
                It.IsAny<string>(),
                It.IsAny<ParseMode?>(),
                It.IsAny<IEnumerable<MessageEntity>?>(),
                It.IsAny<bool?>(),
                It.IsAny<InlineKeyboardMarkup?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Throw_WhenInvalidParam()
    {
        await sut.Invoking(s => s.Execute(CancellationToken.None))
            .Should().ThrowAsync<DomainException>();
    }
}