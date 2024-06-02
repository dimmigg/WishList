using AutoMapper;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Domain.Mapper;

public class WishListProfile : Profile
{
    public WishListProfile()
    {
        CreateMap<TelegramUser, User>().ReverseMap();
    }
}