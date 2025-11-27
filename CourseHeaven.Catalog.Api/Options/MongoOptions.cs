using System.ComponentModel.DataAnnotations;

namespace CourseHeaven.Catalog.Api.Options;

public class MongoOptions
{
    [Required] public string DatabaseName { get; set; } = null!;
    [Required] public string ConnectionString { get; set; } = null!;
}