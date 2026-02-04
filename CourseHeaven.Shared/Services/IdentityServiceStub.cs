namespace CourseHeaven.Shared.Services;

public class IdentityServiceStub : IIdentityService
{
    public Guid UserId => Guid.Parse("07323142-beb9-49ae-a872-efb5b080a490");
    public string UserName => "StubUser";
}