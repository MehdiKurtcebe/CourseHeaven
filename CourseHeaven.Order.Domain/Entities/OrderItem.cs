namespace CourseHeaven.Order.Domain.Entities;

public class OrderItem : BaseEntity<int>
{
    public Guid ProductId { get; set; }

    public required string ProductName
    {
        get;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(ProductName));

            field = value;
        }
    }

    public decimal ProductPrice
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            field = value;
        }
    }

    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public void ApplyDiscount(decimal discountRate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(discountRate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(discountRate, 1);

        ProductPrice -= ProductPrice * discountRate;
    }

    public bool IsSameItem(OrderItem item)
    {
        return ProductId == item.ProductId;
    }
}