using CourseHeaven.Discount.Api;
using CourseHeaven.Shared.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddCommonServiceExtension(typeof(DiscountAssembly));
builder.Services.AddVersioningExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();