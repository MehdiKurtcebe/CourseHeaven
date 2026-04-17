namespace CourseHeaven.Order.Application.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitTransactionAsync(CancellationToken cancellationToken);
}