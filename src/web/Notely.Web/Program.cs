using Notely.Web;
using Notely.Web.Components;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Notely.Infrastructure;
using Notely.Infrastructure.Persistence;
using Notely.Core.Application;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server components only
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLocalization();

// Add layer dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices(); // Add Web layer services

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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

// Map API Controllers
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
