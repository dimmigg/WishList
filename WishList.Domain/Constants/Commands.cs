namespace WishList.Domain.Constants;

public static class Commands
{
    public const string Main = "main";
    public const string HowToFindMe = "htfm";
    public const string SelfDeleteButton = "sdb";
    public const string Reserved = "r";
    
    public const string WishLists = "mwl";
    public const string WishListInfo = "mwli";
    public const string WishListParams = "mwlp";
    public const string WishListEditNameRequest = "mwlenr";
    public const string WishListEditName = "mwlen";
    public const string WishListEditSecurityRequest = "mwlsr";
    public const string WishListDeleteRequest = "mwldr";
    public const string WishListDelete = "mwld";
    public const string WishListAddRequest = "mwlar";
    public const string WishListAdd = "mwla";
    
    public const string Presents = "mp";
    public const string PresentInfo = "mpi";
    public const string PresentEditNameRequest = "mpenr";
    public const string PresentEditName = "mpen";
    public const string PresentEditReferenceRequest = "mperr";
    public const string PresentEditReference = "mper";
    public const string PresentEditCommentRequest = "mpecr";
    public const string PresentEditComment = "mpec";
    public const string PresentDeleteRequest = "mpdr";
    public const string PresentDelete = "mpd";
    public const string PresentAddRequest = "mpar";
    public const string PresentAdd = "mpa";
    
    public const string SubscribeUsers = "su";
    public const string SubscribeUserWishLists = "suwl";
    public const string SubscribeWishListInfo = "swli";
    public const string UnsubscribeWishListRequest = "uwlr";
    public const string UnsubscribeWishList = "uwl";
    
    public const string SubscribePresents = "sp";
    public const string SubscribePresentInfo = "spi";
    
    public const string RemoveReservePresent = "rrp";
    public const string ReservePresent = "rp";
    
    public const string UsersFindRequest = "ufr";
    public const string UsersFind = "uf";
    public const string UsersWishListsFindInfo = "uwlfi";
    public const string UsersWishListSubscribeRequest = "uwlsr";
    public const string UsersWishListSubscribe = "uwls";
}