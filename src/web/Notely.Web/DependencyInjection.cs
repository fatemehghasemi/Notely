using Notely.Web.Services.AppServices;
using Notely.Web.Services.State;
using Notely.Web.Services.Notifications;

namespace Notely.Web;

/// <summary>
/// Dependency injection configuration for Web layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        // Register AppServices
        services.AddScoped<INotesAppService, NotesAppService>();
        
        // Register State services
        services.AddScoped<NotesState>();
        
        // Register Notification services
        services.AddScoped<ToastService>();
        
        // TODO: Add other web-specific services in next phase
        
        return services;
    }
}