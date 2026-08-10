using Asp.Versioning.Builder;

namespace CourseHeaven.Order.Api.Endpoints.Orders;

public static class OrderEndpointExtensions
{
    public static void AddOrderGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/orders").WithTags("Orders")
            .WithApiVersionSet(apiVersionSet)
            .CreateOrderGroupItemEndpoint()
            .GetOrdersGroupItemEndpoint()
            .RequireAuthorization("Password");
    }
}