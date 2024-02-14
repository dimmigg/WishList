using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string dbConnectionString)
    {
        services
            .AddScoped<IUserStorage, UserStorage>()
            .AddScoped<IWishListStorage, WishListStorage>()
            .AddDbContextPool<WishListDbContext>(options => options
            .UseNpgsql(dbConnectionString));
        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(WishListDbContext))));
        return services;
    }
}