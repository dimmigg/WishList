using AutoMapper;
using Telegram.Bot.Types;
using WishList.Domain.Models;
using WishList.Storage.Entities;

namespace WishList.Domain.Mapper;

public class WishListProfile : Profile
{
    public WishListProfile()
    {
        CreateMap<TelegramUser, RegisteredUser>().ReverseMap();
        CreateMap<TelegramUser, User>().ReverseMap();
        CreateMap<Storage.Entities.WishList, Models.WishList>().ReverseMap();
    }
}