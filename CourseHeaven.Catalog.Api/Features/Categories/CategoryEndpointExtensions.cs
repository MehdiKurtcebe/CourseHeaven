using CourseHeaven.Catalog.Api.Features.Categories.Create;
using CourseHeaven.Catalog.Api.Features.Categories.Delete;
using CourseHeaven.Catalog.Api.Features.Categories.GetAll;
using CourseHeaven.Catalog.Api.Features.Categories.GetById;
using CourseHeaven.Catalog.Api.Features.Categories.Update;

namespace CourseHeaven.Catalog.Api.Features.Categories;

public static class CategoryEndpointExtensions
{
    public static void AddCategoryGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/categories")
            .WithTags("Categories")
            .CreateCategoryGroupItemEndpoint()
            .GetAllCategoriesGroupItemEndpoint()
            .GetCategoryByIdGroupItemEndpoint()
            .DeleteCategoryGroupItemEndpoint()
            .UpdateCategoryGroupItemEndpoint();
    }
}