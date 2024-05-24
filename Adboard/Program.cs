using DOOH.Adboard;
using DOOH.Adboard.Services;
using DOOH.Adboard.Workers;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Runtime;

var builder = Host.CreateApplicationBuilder(args);

// Set GC settings
GCSettings.LatencyMode = GCLatencyMode.LowLatency;
GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

// Initialize LibVLCSharp core
Core.Initialize();
builder.Services.AddTransient(sp =>
{
    var httpClient = new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("Service:URL") ?? "https://doohfy.com") };
    //var userName = builder.Configuration.GetValue<string>("Service:UserName");
    //var password = builder.Configuration.GetValue<string>("Service:Password");
    //var applicationUser = new DOOH.Server.Models.ApplicationUser { Name = userName, Password = password };
    //var json = System.Text.Json.JsonSerializer.Serialize(applicationUser);
    //var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    //var response = httpClient.PostAsync("/Account/Login", content).Result;
    //var cookies = response.Headers.GetValues("Set-Cookie");
    //--------------------------
    //var cookieContainer = new System.Net.CookieContainer();
    //foreach (var cookie in cookies)
    //{
    //    var parts = cookie.Split(';');
    //    var nameValue = parts[0].Split('=');
    //    var name = nameValue[0];
    //    var value = nameValue[1];
    //    var domain = parts[1].Split('=')[1];
    //    var path = parts[2].Split('=')[1];
    //    cookieContainer.Add(new System.Net.Cookie(name, value, path, domain));
    //}
    //--------------------------
    //httpClient.DefaultRequestHeaders.Add("Cookie", string.Join(";", cookies));
    //httpClient.DefaultRequestHeaders.Add("Referer", builder.Configuration.GetValue<string>("Service:URL"));
    //httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
    //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");

    return httpClient;
});
builder.Services.AddHostedService<PlaybackWorker>();
builder.Services.AddSingleton<DOOHDBService>();
builder.Services.AddSingleton<InterloopService>();
builder.Services.AddSingleton<AdService>();
//builder.Services.AddSingleton<CameraService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

var host = builder.Build();
host.Run();
