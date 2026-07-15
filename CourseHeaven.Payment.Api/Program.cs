using CourseHeaven.Payment.Api;
using CourseHeaven.Payment.Api.Features.Payments;
using CourseHeaven.Payment.Api.Repositories;
using CourseHeaven.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);
builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("payment-in-memory-db"); });
builder.Services.AddCommonServiceExtension(typeof(PaymentAssembly));
builder.Services.AddVersioningExtension();
builder.Services.AddAuthenticationAuthorizationExtension(builder.Configuration);

var app = builder.Build();

app.AddPaymentGroupEndpointExtension(app.AddVersionSetExtension());
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();