using CourseHeaven.Catalog.Api;
using CourseHeaven.Catalog.Api.Options;
using CourseHeaven.Catalog.Api.Repositories;
using CourseHeaven.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOptionsExtension();
builder.Services.AddDatabaseServiceExtension();
builder.Services.AddCommonServiceExtension(typeof(CatalogAssembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.Run();