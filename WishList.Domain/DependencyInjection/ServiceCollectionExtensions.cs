using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WishList.Domain.Mapper;
using WishList.Domain.Received;
using WishList.Domain.Received.CallbackQueryReceived;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Start;
using WishList.Domain.UseCases.SuggestAddWishList;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IReceivedService, ReceivedService>();
        services.AddScoped<IStartUseCase, StartUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<ISuggestAddWishListUseCase, SuggestAddWishListUseCase>();
        services.AddScoped<ICallbackQueryBuilder, CallbackQueryBuilder>();
        services.AddScoped<ISender, Sender>();
        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(UserProfile))));
        return services;
    }
}