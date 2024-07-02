using System.Globalization;
using Radzen;
using DOOH.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using DOOH.Server.Data;
using Microsoft.AspNetCore.Identity;
using DOOH.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Amazon.Runtime;
using Amazon.S3;
using DOOH.Server.Hubs;
using DOOH.Server.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddScoped<DOOH.Server.Services.CDNService>();
builder.Services.AddScoped<DOOH.Server.Services.FFMPEGService>();
builder.Services.AddScoped<DOOH.Server.DOOHDBService>();
builder.Services.AddDbContext<DOOH.Server.Data.DOOHDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DOOHDBConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderDOOHDB = new ODataConventionModelBuilder();
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Adboard>("Adboards");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.AdboardImage>("AdboardImages");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.AdboardNetwork>("AdboardNetworks");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.AdboardStatus>("AdboardStatuses");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.AdboardWifi>("AdboardWifis");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Advertisement>("Advertisements");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Analytic>("Analytics");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Billing>("Billings");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Brand>("Brands");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Campaign>("Campaigns");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.CampaignAdboard>("CampaignAdboards").EntityType.HasKey(entity => new { entity.CampaignId, entity.AdboardId });
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Category>("Categories");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Company>("Companies");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Display>("Displays");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Earning>("Earnings");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Faq>("Faqs");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Motherboard>("Motherboards");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Page>("Pages");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Provider>("Providers");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Schedule>("Schedules");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.ScheduleAdboard>("ScheduleAdboards").EntityType.HasKey(entity => new { entity.ScheduleId, entity.AdboardId });
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Tax>("Taxes");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.Upload>("Uploads");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.UserInformation>("UserInformations");
    oDataBuilderDOOHDB.EntitySet<DOOH.Server.Models.DOOHDB.CampaignCriterion>("CampaignCriteria");
    opt.AddRouteComponents("odata/DOOHDB", oDataBuilderDOOHDB.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
//builder.Services.AddScoped<DOOH.Client.Services.BrowserService>();
builder.Services.AddScoped<DOOH.Client.DOOHDBService>();
builder.Services.AddHttpClient("DOOH.Server").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<DOOH.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DOOHDBConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, DOOH.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddLocalization();
builder.Services.AddScoped<DOOH.Client.DOOHDBService>();
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

// app.Use(async (context, next) =>
// {
//     context.Response.Headers.Append("Cross-Origin-Embedder-Policy", "require-corp");
//     context.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin");
//     await next();
// });
app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseRequestLocalization(options => options.AddSupportedCultures("en", "hi").AddSupportedUICultures("en", "hi").SetDefaultCulture("en"));
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(DOOH.Client._Imports).Assembly);
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Services.CreateScope().ServiceProvider.GetRequiredService<DOOHDBContext>().Database.Migrate();
// app.MapHub<AdboardStatusHub>("/hubs/adboard-status");
app.UseRequestLocalization("en-IN");
app.Run();