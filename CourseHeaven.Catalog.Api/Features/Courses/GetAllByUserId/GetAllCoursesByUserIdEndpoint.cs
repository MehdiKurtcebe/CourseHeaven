using CourseHeaven.Catalog.Api.Features.Courses.Dtos;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Courses.GetAllByUserId;

public record GetAllCoursesByUserIdQuery(Guid UserId) : IRequestByServiceResult<List<CourseDto>>;

public class GetAllCoursesByUserIdHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetAllCoursesByUserIdQuery, ServiceResult<List<CourseDto>>>
{
    public async Task<ServiceResult<List<CourseDto>>> Handle(GetAllCoursesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await context.Courses
            .Where(c => c.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var categories = await context.Categories.ToListAsync(cancellationToken);
        foreach (var course in courses)
            course.Category = categories.First(c => c.Id == course.CategoryId);

        var courseDtos = mapper.Map<List<CourseDto>>(courses);
        return ServiceResult<List<CourseDto>>.SuccessAsOk(courseDtos);
    }
}

public static class GetAllCoursesByUserIdEndpoint
{
    public static RouteGroupBuilder GetAllCoursesByUserIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/user/{userId:guid}",
                async (IMediator mediator, Guid userId) =>
                    (await mediator.Send(new GetAllCoursesByUserIdQuery(userId))).ToGenericResult())
            .WithName("GetAllCoursesByUserId")
            .Produces<List<CourseDto>>();

        return group;
    }
}