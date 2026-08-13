using CourseHeaven.Order.Domain.Entities;

namespace CourseHeaven.Order.Application.Contracts.Repositories;

public interface IOrderRepository : IGenericRepository<Guid, Domain.Entities.Order>
{
    Task<List<Domain.Entities.Order>> GetOrderByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken);

    Task SetStatusAsync(string orderCode, Guid paymentId, OrderStatus status, CancellationToken cancellationToken);
}