namespace CourseHeaven.Bus.Events;

public record CourseImageUploadedEvent(Guid CourseId, string ImageUrl);