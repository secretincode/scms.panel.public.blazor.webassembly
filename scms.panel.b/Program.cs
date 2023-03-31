global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using scms.panel.b;
using scms.panel.b.Abstractions;
using scms.panel.b.Abstractions.Helper;
using scms.panel.b.Services;
using scms.panel.b.Services.AuthStateProviders;
using scms.panel.b.Services.Helper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredModal();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7035/api/") });
builder.Services.AddSingleton<IHelper, Helper>();
builder.Services.AddSingleton<IExternalLoginUrlService, ExternalLoginUrlService>();
builder.Services.AddSingleton<IJwtExtensions, JwtExtensions>();
builder.Services.AddScoped<IJwtHttpService, JwtHttpService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
