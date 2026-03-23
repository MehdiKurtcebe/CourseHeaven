namespace CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;

public record AddBasketItemCommand(Guid CourseId, string CourseName, decimal CoursePrice, string? CourseImageUrl)
    : IRequestByServiceResult;