using MediatR;
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
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishListInfo;
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishLists;
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
            Commands.MAIN => new MainCommand(param),
            Commands.HOW_TO_FIND_ME => new HowToFindMeCommand(param),
            
            Commands.SELF_DELETE_BUTTON => new SelfDeleteButtonCommand(param),
            
            Commands.MY_WISH_LISTS => new MyWishListsCommand(param),
            Commands.MY_WISH_LIST_INFO => new MyWishListInfoCommand(param),
            Commands.MY_WISH_LIST_PARAMS => new MyWishListParamsCommand(param),
            Commands.MY_WISH_LIST_EDIT_NAME_REQUEST => new MyWishListEditNameRequestCommand(param),
            Commands.MY_WISH_LIST_EDIT_NAME => new MyWishListEditNameCommand(param),
            Commands.MY_WISH_LIST_EDIT_SECURITY_REQUEST => new NotImplementedCommand(param),
            Commands.MY_WISH_LIST_DELETE_REQUEST => new MyWishListDeleteRequestCommand(param),
            Commands.MY_WISH_LIST_DELETE => new MyWishListDeleteCommand(param),
            Commands.MY_WISH_LIST_ADD_REQUEST => new MyWishListAddRequestCommand(param),
            Commands.MY_WISH_LIST_ADD => new MyWishListAddCommand(param),
            
            Commands.MY_PRESENTS => new MyPresentsCommand(param),
            Commands.MY_PRESENT_INFO => new MyPresentInfoCommand(param),
            Commands.MY_PRESENT_EDIT_NAME_REQUEST => new MyPresentEditNameRequestCommand(param),
            Commands.MY_PRESENT_EDIT_NAME => new MyPresentEditNameCommand(param),
            Commands.MY_PRESENT_EDIT_REFERENCE_REQUEST => new MyPresentEditReferenceRequestCommand(param),
            Commands.MY_PRESENT_EDIT_REFERENCE => new MyPresentEditReferenceCommand(param),
            Commands.MY_PRESENT_EDIT_COMMENT_REQUEST => new MyPresentEditCommentRequestCommand(param),
            Commands.MY_PRESENT_EDIT_COMMENT => new MyPresentEditCommentCommand(param),
            Commands.MY_PRESENT_DELETE_REQUEST => new MyPresentDeleteRequestCommand(param),
            Commands.MY_PRESENT_DELETE => new MyPresentDeleteCommand(param),
            Commands.MY_PRESENT_ADD_REQUEST => new MyPresentAddRequestCommand(param),
            Commands.MY_PRESENT_ADD => new MyPresentAddCommand(param),
            
            Commands.SUBSCRIBE_WISH_LISTS => new SubscribeWishListsCommand(param),
            Commands.SUBSCRIBE_WISH_LIST_INFO => new SubscribeWishListInfoCommand(param),
            Commands.UNSUBSCRIBE_WISH_LIST_REQUEST => new UnsubscribeWishListRequestCommand(param),
            Commands.UNSUBSCRIBE_WISH_LIST => new UnsubscribeWishListCommand(param),
            
            Commands.SUBSCRIBE_PRESENTS => new SubscribePresentsCommand(param),
            Commands.SUBSCRIBE_PRESENT_INFO => new SubscribePresentInfoCommand(param),
            Commands.REMOVE_RESERVE_PRESENT => new RemoveReservePresentCommand(param),
            Commands.RESERVE_PRESENT => new ReservePresentCommand(param),
            
            Commands.USERS_FIND_REQUEST => new UsersFindRequestCommand(param),
            Commands.USERS_FIND => new UsersFindCommand(param),
            Commands.USERS_WISH_LISTS_FIND_INFO => new UserWishListsFindInfoCommand(param),
            Commands.USERS_WISH_LIST_SUBSCRIBE_REQUEST => new UserWishListSubscribeRequestCommand(param),
            Commands.USERS_WISH_LIST_SUBSCRIBE => new UserWishListSubscribeCommand(param),
            
            _ => throw new DomainException($"Команда {param.LastCommand} не распознана ")
        };
    }
}