using CourseHeaven.Order.Api.Endpoints.Orders;
using CourseHeaven.Order.Application;
using CourseHeaven.Order.Application.Contracts.Repositories;
using CourseHeaven.Order.Application.Contracts.UnitOfWork;
using CourseHeaven.Order.Persistence;
using CourseHeaven.Order.Persistence.Repositories;
using CourseHeaven.Order.Persistence.UnitOfWork;
using CourseHeaven.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
string[] versions = ["v1"];
foreach (var version in versions) builder.Services.AddOpenApi(version);

builder.Services.AddCommonServiceExtension(typeof(OrderApplicationAssembly));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddVersioningExtension();

var app = builder.Build();

app.AddOrderGroupEndpointExtension(app.AddVersionSetExtension());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options => { options.AddDocuments(versions); });
}

app.Run();