using CourseHeaven.Bus.Commands;
using CourseHeaven.Bus.Events;
using MassTransit;
using Microsoft.Extensions.FileProviders;

namespace CourseHeaven.File.Api.Consumers;

public class UploadCourseImageCommandConsumer(IServiceProvider serviceProvider) : IConsumer<UploadCourseImageCommand>
{
    public async Task Consume(ConsumeContext<UploadCourseImageCommand> context)
    {
        using var scope = serviceProvider.CreateScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(context.Message.FileName)}";
        var uploadPath = Path.Combine(fileProvider.GetFileInfo("files").PhysicalPath!, newFileName);

        await System.IO.File.WriteAllBytesAsync(uploadPath, context.Message.Image);

        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        await publishEndpoint.Publish(new CourseImageUploadedEvent(context.Message.CourseId, $"files/{newFileName}"));
    }
}