using CourseHeaven.Basket.Api;
using CourseHeaven.Basket.Api.Consumers;
using CourseHeaven.Basket.Api.Features.Baskets;
using CourseHeaven.Bus;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddCommonServiceExtension(typeof(BasketAssembly));
builder.Services.AddMassTransitExtension(builder.Configuration, typeof(OrderCreatedEventConsumer),
    "basket-microservice.order-created.queue");
builder.Services.AddScoped<BasketService>();
builder.Services.AddVersioningExtension();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddAuthenticationAuthorizationExtension(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(x => { });

app.AddBasketGroupEndpointExtension(app.AddVersionSetExtension());
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();