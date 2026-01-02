using Notely.Web.Services.AppServices;
using Notely.Web.Services.State;
using Notely.Web.Services.Notifications;

namespace Notely.Web;
public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<INotesAppService, NotesAppService>();
        services.AddScoped<ITagsAppService, TagsAppService>();
        
        services.AddScoped<NotesState>();
        
        services.AddScoped<ToastService>();
        
        
        return services;
    }
}
