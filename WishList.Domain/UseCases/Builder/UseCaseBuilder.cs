using MediatR;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.UseCases.Main.HowToFindMe;
using WishList.Domain.UseCases.Main.Main;
using WishList.Domain.UseCases.MyPresents.MyPresentAdd;
using WishList.Domain.UseCases.MyPresents.MyPresentAddRequest;
using WishList.Domain.UseCases.MyPresents.MyPresentDelete;
using WishList.Domain.UseCases.MyPresents.MyPresentDeleteRequest;
using WishList.Domain.UseCases.MyPresents.MyPresentEditComment;
using WishList.Domain.UseCases.MyPresents.MyPresentEditCommentRequest;
using WishList.Domain.UseCases.MyPresents.MyPresentEditName;
using WishList.Domain.UseCases.MyPresents.MyPresentEditNameRequest;
using WishList.Domain.UseCases.MyPresents.MyPresentEditReference;
using WishList.Domain.UseCases.MyPresents.MyPresentEditReferenceRequest;
using WishList.Domain.UseCases.MyPresents.MyPresentInfo;
using WishList.Domain.UseCases.MyPresents.MyPresents;
using WishList.Domain.UseCases.MyWishLists.MyWishListAdd;
using WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;
using WishList.Domain.UseCases.MyWishLists.MyWishListDelete;
using WishList.Domain.UseCases.MyWishLists.MyWishListDeleteRequest;
using WishList.Domain.UseCases.MyWishLists.MyWishListEditName;
using WishList.Domain.UseCases.MyWishLists.MyWishListEditNameRequest;
using WishList.Domain.UseCases.MyWishLists.MyWishListInfo;
using WishList.Domain.UseCases.MyWishLists.MyWishListParams;
using WishList.Domain.UseCases.MyWishLists.MyWishLists;
using WishList.Domain.UseCases.Other.NotImplemented;
using WishList.Domain.UseCases.Other.SelfDeleteButton;
using WishList.Domain.UseCases.SubscribePresents.RemoveReservePresent;
using WishList.Domain.UseCases.SubscribePresents.ReservePresent;
using WishList.Domain.UseCases.SubscribePresents.SubscribePresentInfo;
using WishList.Domain.UseCases.SubscribePresents.SubscribePresents;
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeUsers;
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeUserWishLists;
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishListInfo;
using WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishList;
using WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishListRequest;
using WishList.Domain.UseCases.Users.UsersFind;
using WishList.Domain.UseCases.Users.UsersFindRequest;
using WishList.Domain.UseCases.Users.UserWishListsFindInfo;
using WishList.Domain.UseCases.Users.UserWishListSubscribe;
using WishList.Domain.UseCases.Users.UserWishListSubscribeRequest;

namespace WishList.Domain.UseCases.Builder;

public class UseCaseBuilder : IUseCaseBuilder
{
    private readonly IDictionary<string, Func<UseCaseParam, IRequest>> _commandMap;

    public UseCaseBuilder()
    {
        _commandMap = new Dictionary<string, Func<UseCaseParam, IRequest>>()
        {
            { Commands.Main, param => new MainCommand(param) },
            { Commands.HowToFindMe, param => new HowToFindMeCommand(param) },
            { Commands.SelfDeleteButton, param => new SelfDeleteButtonCommand(param) },
            { Commands.WishLists, param => new MyWishListsCommand(param) },
            { Commands.WishListInfo, param => new MyWishListInfoCommand(param) },
            { Commands.WishListParams, param => new MyWishListParamsCommand(param) },
            { Commands.WishListEditNameRequest, param => new MyWishListEditNameRequestCommand(param) },
            { Commands.WishListEditName, param => new MyWishListEditNameCommand(param) },
            { Commands.WishListEditSecurityRequest, param => new NotImplementedCommand(param) },
            { Commands.WishListDeleteRequest, param => new MyWishListDeleteRequestCommand(param) },
            { Commands.WishListDelete, param => new MyWishListDeleteCommand(param) },
            { Commands.WishListAddRequest, param => new MyWishListAddRequestCommand(param) },
            { Commands.WishListAdd, param => new MyWishListAddCommand(param) },
            { Commands.Presents, param => new MyPresentsCommand(param) },
            { Commands.PresentInfo, param => new MyPresentInfoCommand(param) },
            { Commands.PresentEditNameRequest, param => new MyPresentEditNameRequestCommand(param) },
            { Commands.PresentEditName, param => new MyPresentEditNameCommand(param) },
            { Commands.PresentEditReferenceRequest, param => new MyPresentEditReferenceRequestCommand(param) },
            { Commands.PresentEditReference, param => new MyPresentEditReferenceCommand(param) },
            { Commands.PresentEditCommentRequest, param => new MyPresentEditCommentRequestCommand(param) },
            { Commands.PresentEditComment, param => new MyPresentEditCommentCommand(param) },
            { Commands.PresentDeleteRequest, param => new MyPresentDeleteRequestCommand(param) },
            { Commands.PresentDelete, param => new MyPresentDeleteCommand(param) },
            { Commands.PresentAddRequest, param => new MyPresentAddRequestCommand(param) },
            { Commands.PresentAdd, param => new MyPresentAddCommand(param) },
            { Commands.SubscribeUsers, param => new SubscribeUsersCommand(param) },
            { Commands.SubscribeUserWishLists, param => new SubscribeUserWishListsCommand(param) },
            { Commands.SubscribeWishListInfo, param => new SubscribeWishListInfoCommand(param) },
            { Commands.UnsubscribeWishListRequest, param => new UnsubscribeWishListRequestCommand(param) },
            { Commands.UnsubscribeWishList, param => new UnsubscribeWishListCommand(param) },
            { Commands.SubscribePresents, param => new SubscribePresentsCommand(param) },
            { Commands.SubscribePresentInfo, param => new SubscribePresentInfoCommand(param) },
            { Commands.RemoveReservePresent, param => new RemoveReservePresentCommand(param) },
            { Commands.ReservePresent, param => new ReservePresentCommand(param) },
            { Commands.UsersFindRequest, param => new UsersFindRequestCommand(param) },
            { Commands.UsersFind, param => new UsersFindCommand(param) },
            { Commands.UsersWishListsFindInfo, param => new UserWishListsFindInfoCommand(param) },
            { Commands.UsersWishListSubscribeRequest, param => new UserWishListSubscribeRequestCommand(param) },
            { Commands.UsersWishListSubscribe, param => new UserWishListSubscribeCommand(param) }
        };
    }

    public IRequest Build(UseCaseParam param)
    {
        if (_commandMap.TryGetValue(param.LastCommand, out var commandFactory))
        {
            return commandFactory(param);
        }

        throw new DomainException($"Команда {param.LastCommand} не распознана.");
    }
}