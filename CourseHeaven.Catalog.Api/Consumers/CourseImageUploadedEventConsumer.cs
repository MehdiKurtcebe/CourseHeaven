using CourseHeaven.Bus.Events;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Consumers;

public class CourseImageUploadedEventConsumer(IServiceProvider serviceProvider) : IConsumer<CourseImageUploadedEvent>
{
    public async Task Consume(ConsumeContext<CourseImageUploadedEvent> context)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var course = await dbContext.Courses.FindAsync(context.Message.CourseId);
        if (course is null) throw new Exception("Course not found");
        course.ImageUrl = context.Message.ImageUrl;
        await dbContext.SaveChangesAsync();
    }
}