namespace CourseHeaven.Web.Options;

public class MicroserviceOptions
{
    public required MicroserviceOptionsItem Catalog { get; set; }
}

public class MicroserviceOptionsItem
{
    public required string BaseAddress { get; set; }
}