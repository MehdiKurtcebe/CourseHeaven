using CourseHeaven.Web.DelegateHandlers;
using CourseHeaven.Web.Extensions;
using CourseHeaven.Web.Options;
using CourseHeaven.Web.Pages.Auth.SignIn;
using CourseHeaven.Web.Pages.Auth.SignUp;
using CourseHeaven.Web.Services;
using CourseHeaven.Web.Services.Refit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddOptionsExtension();

builder.Services.AddHttpClient<SignUpService>();
builder.Services.AddHttpClient<SignInService>();
builder.Services.AddHttpClient<TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<ICatalogRefitService>().ConfigureHttpClient(client =>
    {
        var microserviceOptions =
            builder.Configuration.GetSection(nameof(MicroserviceOptions)).Get<MicroserviceOptions>();
        client.BaseAddress = new Uri(microserviceOptions!.Catalog.BaseAddress);
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();