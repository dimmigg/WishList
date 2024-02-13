using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WishList.Storage.Storages.Users;

namespace WishList.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, string dbConnectionString)
    {
        services
            .AddScoped<IUserStorage, UserStorage>()
            .AddDbContextPool<WishListDbContext>(options => options
            .UseNpgsql(dbConnectionString));
        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(WishListDbContext))));
        return services;
    }
}