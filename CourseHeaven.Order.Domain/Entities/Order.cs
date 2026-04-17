using MassTransit;

namespace CourseHeaven.Order.Domain.Entities;

public class Order : BaseEntity<Guid>
{
    public required string OrderCode { get; set; }

    public OrderStatus Status { get; set; }

    public Guid BuyerId { get; set; }

    public List<OrderItem> OrderItems { get; set; } = [];

    public decimal TotalPrice { get; set; }

    public decimal DiscountRate { get; set; }

    public required string InvoiceAddress { get; set; }

    public Guid? PaymentId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public static Order GenerateUnpaidOrder(Guid buyerId, string invoiceAddress, decimal discountRate = 0)
    {
        return new Order
        {
            Id = NewId.NextGuid(),
            OrderCode = GenerateOrderCode(),
            Status = OrderStatus.WaitingForPayment,
            BuyerId = buyerId,
            TotalPrice = 0,
            DiscountRate = discountRate,
            InvoiceAddress = invoiceAddress,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void AddOrderItem(Guid productId, string productName, decimal productPrice)
    {
        var orderItem = new OrderItem
        {
            ProductId = productId,
            ProductName = productName,
            ProductPrice = productPrice * (1 - DiscountRate)
        };

        OrderItems.Add(orderItem);
        CalculateTotalPrice();
    }

    public void ApplyDiscount(decimal discountRate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(discountRate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(discountRate, 1);

        DiscountRate = discountRate;
        OrderItems.ForEach(item => item.ProductPrice *= 1 - discountRate);
        CalculateTotalPrice();
    }

    public void SetStatusToPaid(Guid paymentId)
    {
        PaymentId = paymentId;
        Status = OrderStatus.Paid;
    }

    private static string GenerateOrderCode()
    {
        return NewId.NextSequentialGuid().ToString();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = OrderItems.Sum(item => item.ProductPrice);
    }
}

public enum OrderStatus
{
    WaitingForPayment = 1,
    Paid = 2,
    Cancelled = 3
}