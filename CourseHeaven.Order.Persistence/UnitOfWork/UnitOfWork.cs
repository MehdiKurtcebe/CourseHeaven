using CourseHeaven.Order.Application.Contracts.UnitOfWork;

namespace CourseHeaven.Order.Persistence.UnitOfWork;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return context.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return context.Database.CommitTransactionAsync(cancellationToken);
    }
}