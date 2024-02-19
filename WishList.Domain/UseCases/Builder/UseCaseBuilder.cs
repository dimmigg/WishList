using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.MyPresents;
using WishList.Domain.UseCases.MyWishLists;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Builder;

public class UseCaseBuilder(
    IWishListStorage wishListStorage,
    IUserStorage userStorage,
    IPresentStorage presentStorage,
    ISender sender)
    : IUseCaseBuilder
{
    public IUseCase Build(UseCaseParam param)
    {
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1].Split("<?>")[0];
        return lastCommand switch
        {
            "main" => new MainUseCase(param, sender),
            "my-wish-lists" => new MyWishListsUseCase(param, sender, wishListStorage),
            "my-wish-list-name-request" => new MyWishListNameRequestUseCase(param, sender, userStorage),
            "my-wish-list-add" => new MyWishListAddUseCase(param, sender, wishListStorage, userStorage),
            "my-wish-list-info" => new MyWishListInfoUseCase(param, sender, wishListStorage),
            "my-presents" => new MyPresentsUseCase(param, sender, wishListStorage, presentStorage),
            "my-present-info" => new MyPresentInfoUseCase(param, sender, presentStorage),
            "my-present-add-request" => new MyPresentAddRequestUseCase(param, sender, userStorage),
            "my-present-add" => new MyPresentAddUseCase(param, sender, presentStorage, userStorage),
            "my-present-edit-name-request" => new MyPresentEditNameRequestUseCase(param, sender, userStorage),
            "my-present-edit-name" => new MyPresentEditNameUseCase(param, sender, presentStorage, userStorage),
            "my-present-edit-reference-request" => new MyPresentEditReferenceRequestUseCase(param, sender, userStorage),
            "my-present-edit-reference" => new MyPresentEditReferenceUseCase(param, sender, presentStorage, userStorage),
            "my-present-edit-comment-request" => new MyPresentEditCommentRequestUseCase(param, sender, userStorage),
            "my-present-edit-comment" => new MyPresentEditCommentUseCase(param, sender, presentStorage, userStorage),
            "my-present-delete-request" => new MyPresentDeleteRequestUseCase(param, sender,presentStorage),
            "my-present-delete" => new MyPresentDeleteUseCase(param, sender, presentStorage),
            "my-present-new-name" => new MyPresentEditNameRequestUseCase(param, sender, userStorage),
            _ => throw new DomainException("Команда не распознана")
        };
    }
}