using CourseHeaven.Bus;
using CourseHeaven.Catalog.Api;
using CourseHeaven.Catalog.Api.Consumers;
using CourseHeaven.Catalog.Api.Features.Categories;
using CourseHeaven.Catalog.Api.Features.Courses;
using CourseHeaven.Catalog.Api.Options;
using CourseHeaven.Catalog.Api.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddOptionsExtension();
builder.Services.AddDatabaseServiceExtension();
builder.Services.AddCommonServiceExtension(typeof(CatalogAssembly));
builder.Services.AddMassTransitExtension(builder.Configuration, typeof(CourseImageUploadedEventConsumer),
    "catalog-microservice.course-picture-uploaded.queue");
builder.Services.AddVersioningExtension();
builder.Services.AddAuthenticationAuthorizationExtension(builder.Configuration);

var app = builder.Build();

app.AddSeedDataExtension().ContinueWith(x =>
{
    Console.WriteLine(x.IsFaulted ? x.Exception?.Message : "Seed data added successfully.");
});
app.AddCategoryGroupEndpointExtension(app.AddVersionSetExtension());
app.AddCourseGroupEndpointExtension(app.AddVersionSetExtension());
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();