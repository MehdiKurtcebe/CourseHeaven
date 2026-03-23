using CourseHeaven.Discount.Api.Options;
using MongoDB.Driver;

namespace CourseHeaven.Discount.Api.Repositories;

public static class RepositoryExtensions
{
    public static IServiceCollection AddDatabaseServiceExtension(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var options = sp.GetRequiredService<MongoOptions>();
            return new MongoClient(options.ConnectionString);
        });

        services.AddScoped<AppDbContext>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var options = sp.GetRequiredService<MongoOptions>();

            return AppDbContext.Create(mongoClient.GetDatabase(options.DatabaseName));
        });

        return services;
    }
}