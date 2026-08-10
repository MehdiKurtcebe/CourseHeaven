using CourseHeaven.Bus;
using CourseHeaven.Discount.Api;
using CourseHeaven.Discount.Api.Consumers;
using CourseHeaven.Discount.Api.Features.Discounts;
using CourseHeaven.Discount.Api.Options;
using CourseHeaven.Discount.Api.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddOptionsExtension();
builder.Services.AddDatabaseServiceExtension();
builder.Services.AddCommonServiceExtension(typeof(DiscountAssembly));
builder.Services.AddMassTransitExtension(builder.Configuration, typeof(OrderCreatedEventConsumer),
    "discount-microservice.order-created.queue");
builder.Services.AddVersioningExtension();
builder.Services.AddAuthenticationAuthorizationExtension(builder.Configuration);

var app = builder.Build();

app.AddDiscountGroupEndpointExtension(app.AddVersionSetExtension());
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();