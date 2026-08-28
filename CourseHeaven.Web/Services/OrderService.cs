using System.Net;
using CourseHeaven.Web.Extensions;
using CourseHeaven.Web.Pages.Order.Dto;
using CourseHeaven.Web.Pages.Order.ViewModel;
using CourseHeaven.Web.Services.Refit;
using Refit;

namespace CourseHeaven.Web.Services;

public class OrderService(IOrderRefitService orderRefitService, ILogger<OrderService> logger)
{
    public async Task<ServiceResult> CreateOrderAsync(CreateOrderViewModel viewModel)
    {
        var address = viewModel.Address;
        
        var payment = new PaymentDto(viewModel.Payment.CardNumber, viewModel.Payment.CardHolderName,
            viewModel.Payment.ExpiryDate, viewModel.Payment.Cvv, viewModel.TotalPrice);
        
        var orderItems = viewModel.OrderItems.Select(x => new OrderItemDto(x.ProductId, x.ProductName, x.ProductPrice))
            .ToList();
        
        var createOrderRequest = new CreateOrderRequest(viewModel.DiscountRate ?? 0, address, payment, orderItems);
        
        var response = await orderRefitService.CreateOrderAsync(createOrderRequest);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return ServiceResult.FailFromProblemDetails((response.Error as ApiException)!);

            logger.LogProblemDetailsExtension(response.Error as ApiException);
            return ServiceResult.Error("An error occurred while creating the order");
        }

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<List<OrderHistoryViewModel>>> GetHistoryAsync()
    {
        var response = await orderRefitService.GetOrdersAsync();
        if (!response.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(response.Error as ApiException);
            return ServiceResult<List<OrderHistoryViewModel>>.Error(
                "An error occurred while getting the order history");
        }

        var orderHistoryList = new List<OrderHistoryViewModel>();
        
        foreach (var orderResponse in response.Content!)
        {
            var newOrderHistory =
                new OrderHistoryViewModel(orderResponse.CreatedAt.LocalDateTime.ToLongDateString(),
                    orderResponse.TotalPrice.ToString("C"));

            foreach (var orderItem in orderResponse.Items)
                newOrderHistory.AddItem(orderItem.ProductId, orderItem.ProductName, orderItem.ProductPrice);

            orderHistoryList.Add(newOrderHistory);
        }

        return ServiceResult<List<OrderHistoryViewModel>>.Success(orderHistoryList);
    }
}