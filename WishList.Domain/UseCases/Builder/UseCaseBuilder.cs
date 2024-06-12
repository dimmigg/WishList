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

public class UseCaseBuilder()
    : IUseCaseBuilder
{
    public IRequest Build(UseCaseParam param)
    {
        return param.LastCommand switch
        {
            Commands.Main => new MainCommand(param),
            Commands.HowToFindMe => new HowToFindMeCommand(param),
            
            Commands.SelfDeleteButton => new SelfDeleteButtonCommand(param),
            
            Commands.WishLists => new MyWishListsCommand(param),
            Commands.WishListInfo => new MyWishListInfoCommand(param),
            Commands.WishListParams => new MyWishListParamsCommand(param),
            Commands.WishListEditNameRequest => new MyWishListEditNameRequestCommand(param),
            Commands.WishListEditName => new MyWishListEditNameCommand(param),
            Commands.WishListEditSecurityRequest => new NotImplementedCommand(param),
            Commands.WishListDeleteRequest => new MyWishListDeleteRequestCommand(param),
            Commands.WishListDelete => new MyWishListDeleteCommand(param),
            Commands.WishListAddRequest => new MyWishListAddRequestCommand(param),
            Commands.WishListAdd => new MyWishListAddCommand(param),
            
            Commands.Presents => new MyPresentsCommand(param),
            Commands.PresentInfo => new MyPresentInfoCommand(param),
            Commands.PresentEditNameRequest => new MyPresentEditNameRequestCommand(param),
            Commands.PresentEditName => new MyPresentEditNameCommand(param),
            Commands.PresentEditReferenceRequest => new MyPresentEditReferenceRequestCommand(param),
            Commands.PresentEditReference => new MyPresentEditReferenceCommand(param),
            Commands.PresentEditCommentRequest => new MyPresentEditCommentRequestCommand(param),
            Commands.PresentEditComment => new MyPresentEditCommentCommand(param),
            Commands.PresentDeleteRequest => new MyPresentDeleteRequestCommand(param),
            Commands.PresentDelete => new MyPresentDeleteCommand(param),
            Commands.PresentAddRequest => new MyPresentAddRequestCommand(param),
            Commands.PresentAdd => new MyPresentAddCommand(param),
            
            Commands.SubscribeUsers => new SubscribeUsersCommand(param),
            Commands.SubscribeUserWishLists => new SubscribeUserWishListsCommand(param),
            Commands.SubscribeWishListInfo => new SubscribeWishListInfoCommand(param),
            Commands.UnsubscribeWishListRequest => new UnsubscribeWishListRequestCommand(param),
            Commands.UnsubscribeWishList => new UnsubscribeWishListCommand(param),
            
            Commands.SubscribePresents => new SubscribePresentsCommand(param),
            Commands.SubscribePresentInfo => new SubscribePresentInfoCommand(param),
            Commands.RemoveReservePresent => new RemoveReservePresentCommand(param),
            Commands.ReservePresent => new ReservePresentCommand(param),
            
            Commands.UsersFindRequest => new UsersFindRequestCommand(param),
            Commands.UsersFind => new UsersFindCommand(param),
            Commands.UsersWishListsFindInfo => new UserWishListsFindInfoCommand(param),
            Commands.UsersWishListSubscribeRequest => new UserWishListSubscribeRequestCommand(param),
            Commands.UsersWishListSubscribe => new UserWishListSubscribeCommand(param),
            
            _ => throw new DomainException($"Команда {param.LastCommand} не распознана ")
        };
    }
}