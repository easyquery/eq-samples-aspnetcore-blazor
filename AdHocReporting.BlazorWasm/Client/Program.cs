using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using EqDemo.Client;
using EqDemo.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// The app and its API share one origin, so the browser sends the Identity cookie with every request
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Authentication state comes from the server's cookie sign-in (see ServerAuthenticationStateProvider)
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ServerAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ServerAuthenticationStateProvider>());

await builder.Build().RunAsync();
