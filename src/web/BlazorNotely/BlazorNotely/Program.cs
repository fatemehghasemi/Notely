using BlazorNotely.Components;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Notely.Infrastructure;
using Notely.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BlazorNotelyContext>();

    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context);
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
