using CourseHeaven.Order.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseHeaven.Order.Persistence.Repositories;

public class OrderRepository(AppDbContext context)
    : GenericRepository<Guid, Domain.Entities.Order>(context), IOrderRepository
{
    public Task<List<Domain.Entities.Order>> GetOrderByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken)
    {
        return Context.Orders.Include(o => o.OrderItems).Where(o => o.BuyerId == buyerId)
            .OrderByDescending(o => o.CreatedAt).ToListAsync(cancellationToken);
    }
}