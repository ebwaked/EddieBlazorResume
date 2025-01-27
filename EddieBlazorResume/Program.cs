using EddieBlazorResume.Client.Pages;
using EddieBlazorResume.Components;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
//builder.WebHost.UseStaticWebAssets();
builder.Services.AddCors(options =>
{
     options.AddPolicy("origins",
                           policy =>
                           {
                                policy.WithOrigins("http://localhost:32768", "https://localhost:32768", "http://localhost:7071", "https://localhost:7071", 
                                    "http://eddies-resume.azurewebistes.net", "https://eddies-resume.azurewebistes.net", "http://deveddie.com", "https://deveddie.com")
                                                   .AllowAnyHeader()
                                                   .AllowAnyMethod();
                           });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("origins");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);
    //.AddAdditionalAssemblies(typeof(Weather).Assembly);

app.Run();
