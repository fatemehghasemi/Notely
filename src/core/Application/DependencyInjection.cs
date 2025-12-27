using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notely.Core.Application.Common.Behaviors;
using System.Reflection;

namespace Notely.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services;
    }
}
