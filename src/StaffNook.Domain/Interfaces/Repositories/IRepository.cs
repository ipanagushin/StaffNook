using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> Insert(TEntity data, CancellationToken cancellationToken = default);
    Task<TEntity> Update(TEntity data, CancellationToken cancellationToken = default);
    Task Delete(TEntity data, CancellationToken cancellationToken = default);
}