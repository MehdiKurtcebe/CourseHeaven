using System.Collections.Immutable;

namespace CourseHeaven.Web.Pages.Order.ViewModel;

public record OrderHistoryViewModel(string DateTime, string TotalPrice)
{
    private List<OrderItemViewModel> OrderItems { get; } = [];

    public ImmutableList<OrderItemViewModel> GetItems => [.. OrderItems];
    
    public void AddItem(Guid productId, string productName, decimal unitPrice)
    {
        OrderItems.Add(new OrderItemViewModel(productId, productName, unitPrice));
    }
}