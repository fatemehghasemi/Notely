using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notely.Infrastructure.Persistence;

namespace Notely.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlazorNotelyContext>(options =>
        options.UseSqlServer(
            configuration.GetConnectionString("NotelyDb"),
            sql => sql.MigrationsAssembly(
                typeof(BlazorNotelyContext).Assembly.FullName
            )
        ));


        return services;
    }
}
