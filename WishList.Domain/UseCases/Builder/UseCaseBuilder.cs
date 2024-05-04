using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.Received;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Main;
using WishList.Domain.UseCases.MyPresents;
using WishList.Domain.UseCases.MyWishLists;
using WishList.Domain.UseCases.Other;
using WishList.Domain.UseCases.SubscribePresents;
using WishList.Domain.UseCases.SubscribeWishLists;
using WishList.Domain.UseCases.Users;
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
        var lastCommand = param.Command.Split("<?>")[0];
        return lastCommand switch
        {
            "main" => new MainUseCase(param, sender),
            "my-wish-lists" => new MyWishListsUseCase(param, sender, wishListStorage),
            "my-wish-list-name-request" => new MyWishListNameRequestUseCase(param, sender, userStorage),
            "my-wish-list-add" => new MyWishListAddUseCase(param, sender, wishListStorage, userStorage),
            "my-wish-list-info" => new MyWishListInfoUseCase(param, sender, wishListStorage),
            "my-wish-list-params" => new MyWishListParamsUseCase(param, sender, wishListStorage),
            "my-wish-list-edit-name-request" => new MyWishListEditNameRequestUseCase(param, sender, wishListStorage, userStorage),
            "my-wish-list-edit-name" => new MyWishListEditNameUseCase(param, sender, wishListStorage, userStorage),
            "my-wish-list-security-request" => new NotImplementedUseCase(param, sender),
            "my-wish-list-delete-request" => new MyWishListDeleteRequestUseCase(param, sender, wishListStorage),
            "my-wish-list-delete" => new MyWishListDeleteUseCase(param, sender, wishListStorage),
            
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
            "my-present-delete-request" => new MyPresentDeleteRequestUseCase(param, sender, presentStorage),
            "my-present-delete" => new MyPresentDeleteUseCase(param, sender, presentStorage),
            "my-present-new-name" => new MyPresentEditNameRequestUseCase(param, sender, userStorage),
            "user-wish-lists-find" => new UserWishListsFindInfoUseCase(param, sender, wishListStorage, userStorage),
            "user-wish-list-subscribe-request" => new UserWishListSubscribeRequestUseCase(param, sender, wishListStorage, userStorage),
            "user-wish-list-subscribe" => new UserWishListSubscribeUseCase(param, sender, wishListStorage, userStorage),
            
            "subscribe-wish-lists" => new SubscribeWishListsUseCase(param, sender, wishListStorage),
            "subscribe-wish-list-info" => new SubscribeWishListInfoUseCase(param, sender, wishListStorage, presentStorage),
            "unsubscribe-wish-list-request" => new UnsubscribeWishListRequestUseCase(param, sender, wishListStorage),
            "unsubscribe-wish-list" => new UnsubscribeWishListUseCase(param, sender, wishListStorage),
            "subscribe-presents" => new SubscribePresentsUseCase(param, sender, presentStorage, wishListStorage),
            "subscribe-present-info" => new SubscribePresentInfoUseCase(param, sender, presentStorage),
            "reserve-present" => new ReservePresentUseCase(param, sender, presentStorage),
            "remove-reserve-present" => new RemoveReservePresentUseCase(param, sender, presentStorage),
            
            "users-find-request" => new UsersFindRequestUseCase(param, sender, userStorage),
            "users-find" => new UsersFindUseCase(param, sender, userStorage),
            "how-to-find-me" => new HowToFindMeUseCase(param, sender),
            _ => throw new DomainException($"Команда не распознана {lastCommand}")
        };
    }
}