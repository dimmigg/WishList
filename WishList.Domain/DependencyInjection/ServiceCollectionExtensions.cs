using Microsoft.Extensions.DependencyInjection;
using WishList.Domain.Received;
using WishList.Domain.UseCases.Start;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IReceivedService, ReceivedService>();
        services.AddScoped<IStartUseCase, StartUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        return services;
    }
}