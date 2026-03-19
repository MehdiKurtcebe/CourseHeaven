using CourseHeaven.Basket.Api;
using CourseHeaven.Basket.Api.Features.Baskets;
using CourseHeaven.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExtension(typeof(BasketAssembly));
builder.Services.AddScoped<BasketService>();
builder.Services.AddVersioningExtension();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

app.AddBasketGroupEndpointExtension(app.AddVersionSetExtension());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.Run();