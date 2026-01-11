using CourseHeaven.Catalog.Api.Features.Categories;
using CourseHeaven.Catalog.Api.Features.Courses;

namespace CourseHeaven.Catalog.Api.Repositories;

public static class SeedData
{
    public static async Task AddSeedDataExtension(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Categories.Any())
        {
            dbContext.Categories.AddRange(
                new Category { Id = NewId.NextSequentialGuid(), Name = "Programming" },
                new Category { Id = NewId.NextSequentialGuid(), Name = "Design" },
                new Category { Id = NewId.NextSequentialGuid(), Name = "Marketing" }
            );
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Courses.Any())
        {
            var programmingCategory = await dbContext.Categories.FirstAsync(c => c.Name == "Programming");
            var designCategory = await dbContext.Categories.FirstAsync(c => c.Name == "Design");
            var marketingCategory = await dbContext.Categories.FirstAsync(c => c.Name == "Marketing");

            var randomUserId = NewId.NextSequentialGuid();

            dbContext.Courses.AddRange(
                new Course
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Learn C# Programming",
                    Description = "A comprehensive course on C# programming.",
                    Price = 29.99M,
                    UserId = randomUserId,
                    CreatedAt = DateTime.UtcNow,
                    Feature = new Feature { Duration = 10, Rating = 4.1D, EducatorFullName = "John Doe" },
                    CategoryId = programmingCategory.Id
                },
                new Course
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Introduction to Graphic Design",
                    Description = "Basics of graphic design principles and tools.",
                    Price = 19.99M,
                    UserId = randomUserId,
                    CreatedAt = DateTime.UtcNow,
                    Feature = new Feature { Duration = 8, Rating = 4.9D, EducatorFullName = "Jane Smith" },
                    CategoryId = designCategory.Id
                },
                new Course
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Digital Marketing 101",
                    Description = "Fundamentals of digital marketing strategies.",
                    Price = 24.99M,
                    UserId = randomUserId,
                    CreatedAt = DateTime.UtcNow,
                    Feature = new Feature { Duration = 12, Rating = 3.8D, EducatorFullName = "Alice Johnson" },
                    CategoryId = marketingCategory.Id
                }
            );
            await dbContext.SaveChangesAsync();
        }
    }
}