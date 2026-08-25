using CourseHeaven.Web.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CourseHeaven.Web.TagHelpers;

public class CourseThumbnailPictureTagHelper(MicroserviceOptions microserviceOptions) : TagHelper
{
    public string? Src { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "img";
        
        if (string.IsNullOrEmpty(Src))
            output.Attributes.SetAttribute("src", "/images/blank_course_thumbnail.jpg");
        else
        {
            var courseThumbnailImagePath = $"{microserviceOptions.File.BaseAddress}/{Src}";
            output.Attributes.SetAttribute("src", courseThumbnailImagePath);
        }

        return base.ProcessAsync(context, output);
    }
}