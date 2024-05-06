using WishList.Domain.Exceptions;
using WishList.Domain.Models;
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
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1].Split("<?>")[0];
        return lastCommand switch
        {
            "main" => new MainUseCase(param, sender),
            "mwl" => new MyWishListsUseCase(param, sender, wishListStorage),
            "mwlnr" => new MyWishListNameRequestUseCase(param, sender, userStorage),
            "mwla" => new MyWishListAddUseCase(param, sender, wishListStorage, userStorage),
            "mwli" => new MyWishListInfoUseCase(param, sender, wishListStorage),
            "mwlp" => new MyWishListParamsUseCase(param, sender, wishListStorage),
            "mwlenr" => new MyWishListEditNameRequestUseCase(param, sender, wishListStorage, userStorage),
            "mwlen" => new MyWishListEditNameUseCase(param, sender, wishListStorage, userStorage),
            "mwlsr" => new NotImplementedUseCase(param, sender),
            "mwldr" => new MyWishListDeleteRequestUseCase(param, sender, wishListStorage),
            "mwld" => new MyWishListDeleteUseCase(param, sender, wishListStorage),
            
            "mp" => new MyPresentsUseCase(param, sender, wishListStorage, presentStorage),
            "mpi" => new MyPresentInfoUseCase(param, sender, presentStorage),
            "mpar" => new MyPresentAddRequestUseCase(param, sender, userStorage),
            "mpa" => new MyPresentAddUseCase(param, sender, presentStorage, userStorage),
            "mpenr" => new MyPresentEditNameRequestUseCase(param, sender, userStorage),
            "mpen" => new MyPresentEditNameUseCase(param, sender, presentStorage, userStorage),
            "mperr" => new MyPresentEditReferenceRequestUseCase(param, sender, userStorage),
            "mper" => new MyPresentEditReferenceUseCase(param, sender, presentStorage, userStorage),
            "mpecr" => new MyPresentEditCommentRequestUseCase(param, sender, userStorage),
            "mpec" => new MyPresentEditCommentUseCase(param, sender, presentStorage, userStorage),
            "mpdr" => new MyPresentDeleteRequestUseCase(param, sender, presentStorage),
            "mpd" => new MyPresentDeleteUseCase(param, sender, presentStorage),
            "mpnn" => new MyPresentEditNameRequestUseCase(param, sender, userStorage),
            "uwlf" => new UserWishListsFindInfoUseCase(param, sender, wishListStorage, userStorage),
            "uwlsr" => new UserWishListSubscribeRequestUseCase(param, sender, wishListStorage, userStorage),
            "uwls" => new UserWishListSubscribeUseCase(param, sender, wishListStorage, userStorage),
            
            "swl" => new SubscribeWishListsUseCase(param, sender, wishListStorage),
            "swli" => new SubscribeWishListInfoUseCase(param, sender, wishListStorage, presentStorage),
            "uwlr" => new UnsubscribeWishListRequestUseCase(param, sender, wishListStorage),
            "uwl" => new UnsubscribeWishListUseCase(param, sender, wishListStorage),
            "sp" => new SubscribePresentsUseCase(param, sender, presentStorage, wishListStorage),
            "spi" => new SubscribePresentInfoUseCase(param, sender, presentStorage),
            "rp" => new ReservePresentUseCase(param, sender, presentStorage),
            "rrp" => new RemoveReservePresentUseCase(param, sender, presentStorage),
            
            "ufr" => new UsersFindRequestUseCase(param, sender, userStorage),
            "uf" => new UsersFindUseCase(param, sender, userStorage),
            "htfm" => new HowToFindMeUseCase(param, sender),
            _ => throw new DomainException($"Команда не распознана {lastCommand}")
        };
    }
}