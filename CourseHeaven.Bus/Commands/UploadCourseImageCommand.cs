namespace CourseHeaven.Bus.Commands;

public record UploadCourseImageCommand(Guid CourseId, byte[] Image, string FileName);