using AutoMapper;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<TelegramUser, User>().ReverseMap();
    }
}