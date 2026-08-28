using System.Globalization;
using CourseHeaven.Web.DelegateHandlers;
using CourseHeaven.Web.ExceptionHandlers;
using CourseHeaven.Web.Extensions;
using CourseHeaven.Web.Options;
using CourseHeaven.Web.Pages.Auth.SignIn;
using CourseHeaven.Web.Pages.Auth.SignUp;
using CourseHeaven.Web.Services;
using CourseHeaven.Web.Services.Refit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "keys")))
    .SetApplicationName("UdemyNewMicroserviceWebProtectionKeys").SetDefaultKeyLifetime(TimeSpan.FromDays(60));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddOptionsExtension();

builder.Services.AddHttpClient<SignUpService>();
builder.Services.AddHttpClient<SignInService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddExceptionHandler<UnauthorizedAccessExceptionHandler>();

builder.Services.AddRefitClient<ICatalogRefitService>().ConfigureHttpClient(client =>
    {
        var microserviceOptions =
            builder.Configuration.GetSection(nameof(MicroserviceOptions)).Get<MicroserviceOptions>();
        client.BaseAddress = new Uri(microserviceOptions!.Catalog.BaseAddress);
    }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddRefitClient<IBasketRefitService>().ConfigureHttpClient(client =>
    {
        var microserviceOptions =
            builder.Configuration.GetSection(nameof(MicroserviceOptions)).Get<MicroserviceOptions>();
        client.BaseAddress = new Uri(microserviceOptions!.Basket.BaseAddress);
    }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddRefitClient<IDiscountRefitService>().ConfigureHttpClient(client =>
    {
        var microserviceOptions =
            builder.Configuration.GetSection(nameof(MicroserviceOptions)).Get<MicroserviceOptions>();
        client.BaseAddress = new Uri(microserviceOptions!.Discount.BaseAddress);
    }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
builder.Services.AddRefitClient<IOrderRefitService>().ConfigureHttpClient(client =>
    {
        var microserviceOptions =
            builder.Configuration.GetSection(nameof(MicroserviceOptions)).Get<MicroserviceOptions>();
        client.BaseAddress = new Uri(microserviceOptions!.Order.BaseAddress);
    }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Auth/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Cookie.Name = "CourseHeavenWebCookie";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = [cultureInfo],
    SupportedUICultures = [cultureInfo]
});

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();