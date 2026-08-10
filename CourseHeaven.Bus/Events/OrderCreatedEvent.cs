namespace CourseHeaven.Bus.Events;

public record OrderCreatedEvent(Guid OrderId, Guid UserId);