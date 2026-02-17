using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Notely.Client;
using Notely.Client.Services.Categories;
using Notely.Client.Services.Notes;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = ResolveApiBaseUrl(builder);
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl, UriKind.Absolute)
});

builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

await builder.Build().RunAsync();

static string ResolveApiBaseUrl(WebAssemblyHostBuilder builder)
{
    var configured = builder.Configuration["ApiBaseUrl"];
    if (!string.IsNullOrWhiteSpace(configured))
    {
        return configured;
    }

    var hostUri = new Uri(builder.HostEnvironment.BaseAddress);
    var fallbackPort = hostUri.Port switch
    {
        5001 => 5000,
        7001 => 7000,
        _ => 5000
    };

    var fallback = new UriBuilder(hostUri)
    {
        Port = fallbackPort,
        Path = string.Empty,
        Query = string.Empty
    };

    return fallback.Uri.ToString();
}
