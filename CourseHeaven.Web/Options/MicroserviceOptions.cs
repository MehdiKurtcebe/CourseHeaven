namespace CourseHeaven.Web.Options;

public class MicroserviceOptions
{
    public required MicroserviceOptionsItem Catalog { get; set; }
    public required MicroserviceOptionsItem File { get; set; }
    public required MicroserviceOptionsItem Basket { get; set; }
    public required MicroserviceOptionsItem Discount { get; set; }
    public required MicroserviceOptionsItem Order { get; set; }
}

public class MicroserviceOptionsItem
{
    public required string BaseAddress { get; set; }
}