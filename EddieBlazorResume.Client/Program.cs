using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

var localUriString = "http://localhost:7071/";
var prodUriString = "https://emailfunction2025.azurewebsites.net/";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(localUriString + "api/") });

await builder.Build().RunAsync();
