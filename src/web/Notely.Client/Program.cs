using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Notely.Client;
using Notely.Client.Services.Notes;
using Notely.Client.Services.Categories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5000") 
});

builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

await builder.Build().RunAsync();
