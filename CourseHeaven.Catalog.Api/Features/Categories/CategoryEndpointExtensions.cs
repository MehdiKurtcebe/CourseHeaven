using CourseHeaven.Catalog.Api.Features.Categories.Create;

namespace CourseHeaven.Catalog.Api.Features.Categories;

public static class CategoryEndpointExtensions
{
    public static void AddCategoryGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/categories").CreateCategoryGroupItemEndpoint();
    }
}