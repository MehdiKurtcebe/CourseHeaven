using System.ComponentModel.DataAnnotations;

namespace CourseHeaven.Discount.Api.Options;

public class MongoOptions
{
    [Required] public string DatabaseName { get; set; } = null!;
    [Required] public string ConnectionString { get; set; } = null!;
}