using EmojiMemory.UI;
using EmojiMemory.UI.Application.Services;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Infrastructure.Storage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHighscore, LocalStorageHighscore>();
builder.Services.AddScoped<GameEngineService>();

await builder.Build().RunAsync();
