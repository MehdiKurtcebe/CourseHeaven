using CourseHeaven.Catalog.Api.Features.Courses.Dtos;
using CourseHeaven.Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Courses.GetById;

public record GetCourseByIdQuery(Guid Id) : IRequestByServiceResult<CourseDto>;

public class GetCourseByIdHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCourseByIdQuery, ServiceResult<CourseDto>>
{
    public async Task<ServiceResult<CourseDto>> Handle(GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id], cancellationToken);
        if (course == null)
            return ServiceResult<CourseDto>.Error("Course not found",
                $"The course with ID {request.Id} was not found.", HttpStatusCode.NotFound);
        
        var category = await context.Categories.FindAsync([course.CategoryId], cancellationToken);
        course.Category = category!;

        var courseAsDto = mapper.Map<CourseDto>(course);
        return ServiceResult<CourseDto>.SuccessAsOk(courseAsDto);
    }
}

public static class GetCourseByIdEndpoint
{
    public static RouteGroupBuilder GetCourseByIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    (await mediator.Send(new GetCourseByIdQuery(id))).ToGenericResult())
            .WithName("GetCourseById")
            .Produces<CourseDto>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }
}