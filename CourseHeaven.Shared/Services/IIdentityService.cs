namespace CourseHeaven.Shared.Services;

public interface IIdentityService
{
    public Guid UserId { get; }
    public string UserName { get; }
}