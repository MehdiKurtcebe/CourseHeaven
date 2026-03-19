using System.Net;
using System.Text.Json;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Extensions;
using CourseHeaven.Shared.Filters;
using FluentValidation;
using MediatR;

namespace CourseHeaven.Basket.Api.Features.Baskets.DeleteBasketItem;

public record DeleteBasketItemCommand(Guid CourseId) : IRequestByServiceResult;

public class DeleteBasketItemCommandHandler(BasketService basketService)
    : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        var basketJson = await basketService.GetBasketFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketJson))
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketJson);
        var basketItemToDelete = basket?.Items.FirstOrDefault(item => item.CourseId == request.CourseId);
        if (basketItemToDelete is null)
            return ServiceResult.Error("Basket item not found", HttpStatusCode.NotFound);

        basket!.Items.Remove(basketItemToDelete);
        await basketService.CreateBasketCacheAsync(basket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public class DeleteBasketItemCommandValidator : AbstractValidator<DeleteBasketItemCommand>
{
    public DeleteBasketItemCommandValidator()
    {
        RuleFor(c => c.CourseId).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}

public static class DeleteBasketItemEndpoint
{
    public static RouteGroupBuilder DeleteBasketItemGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/items/{courseId:guid}",
                async (IMediator mediator, Guid courseId) =>
                    (await mediator.Send(new DeleteBasketItemCommand(courseId))).ToGenericResult())
            .WithName("DeleteBasketItem")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<DeleteBasketItemCommandValidator>>();

        return group;
    }
}