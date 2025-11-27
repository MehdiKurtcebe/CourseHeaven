using CourseHeaven.Catalog.Api.Features.Categories;
using CourseHeaven.Catalog.Api.Features.Courses;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Metadata.Conventions;

namespace CourseHeaven.Catalog.Api.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new CamelCaseElementNameConvention());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
    }
}