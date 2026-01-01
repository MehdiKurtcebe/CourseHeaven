using CourseHeaven.Catalog.Api.Features.Courses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CourseHeaven.Catalog.Api.Repositories;

public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToCollection("courses");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Price).HasPrecision(18, 2);
        builder.Property(c => c.Picture).HasMaxLength(1000);
        builder.OwnsOne(c => c.Feature, featureBuilder =>
        {
            featureBuilder.Property(f => f.EducatorFullName).HasMaxLength(100);
            featureBuilder.Property(f => f.Rating).HasPrecision(2, 1);
        });
        builder.Ignore(c => c.Category);
    }
}