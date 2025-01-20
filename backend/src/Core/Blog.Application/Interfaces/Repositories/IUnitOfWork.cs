using Blog.Domain.Common;

namespace Blog.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Represents a unit of work for managing database operations.
    /// </summary>
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

        /// <summary>
        /// Saves the changes made in the unit of work asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
    }
}