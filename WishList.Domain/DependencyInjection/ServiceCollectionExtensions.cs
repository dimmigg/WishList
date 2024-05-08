using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WishList.Domain.Mapper;
using WishList.Domain.Models;
using WishList.Domain.PipelineBehavior;
using WishList.Domain.Received;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.Builder;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IReceivedService, ReceivedService>();
        services.AddScoped<IUseCaseBuilder, UseCaseBuilder>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<ITelegramSender, TelegramSender.TelegramSender>();
        services.AddAutoMapper(config => config
            .AddMaps(Assembly.GetAssembly(typeof(WishListProfile))));
        services
            .AddMediatR(cfg => cfg
                .AddOpenBehavior(typeof(Validate<,>))
                .AddOpenBehavior(typeof(ServiceFiller<,>))
                .RegisterServicesFromAssemblyContaining<RegisteredUser>());
        return services;
    }
}