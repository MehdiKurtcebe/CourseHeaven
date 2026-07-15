using CourseHeaven.File.Api;
using CourseHeaven.File.Api.Features.File;
using CourseHeaven.Shared.Extensions;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

// Ensure static file folders exist at startup.
var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
var filesPath = Path.Combine(webRootPath, "files");
Directory.CreateDirectory(webRootPath);
Directory.CreateDirectory(filesPath);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
builder.Services.AddCommonServiceExtension(typeof(FileAssembly));
builder.Services.AddVersioningExtension();
builder.Services.AddAuthenticationAuthorizationExtension(builder.Configuration);

var app = builder.Build();
app.AddFileGroupEndpointExtension(app.AddVersionSetExtension());
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();