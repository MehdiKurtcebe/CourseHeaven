using CourseHeaven.Catalog.Api.Features.Categories.Create;
using CourseHeaven.Catalog.Api.Features.Categories.GetAll;

namespace CourseHeaven.Catalog.Api.Features.Categories;

public static class CategoryEndpointExtensions
{
    public static void AddCategoryGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/categories")
            .CreateCategoryGroupItemEndpoint()
            .GetAllCategoryGroupItemEndpoint();
    }
}