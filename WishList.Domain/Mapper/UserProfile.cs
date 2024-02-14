using AutoMapper;
using WishList.Domain.Models;
using WishList.Storage.Entities;

namespace WishList.Domain.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<TelegramUser, RegisteredUser>().ReverseMap();
    }
}