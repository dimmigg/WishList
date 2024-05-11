using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WishList.Storage.Storages.Presents;
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
            .AddScoped<IPresentStorage, PresentStorage>()
            .AddDbContextPool<WishListDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(dbConnectionString);
            });
        
        using var scope = services.BuildServiceProvider().CreateScope();
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<WishListDbContext>();
                dbContext.Database.Migrate();
        return services;
    }
}