#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetList(Func<TEntity, bool>? filter = null, CancellationToken cancellationToken = default);
    Task<TEntity> Insert(TEntity data, CancellationToken cancellationToken = default);
    Task InsertMany(TEntity[] dataList, CancellationToken cancellationToken = default);
    Task<TEntity> Update(TEntity data, CancellationToken cancellationToken = default);
    Task Delete(TEntity data, CancellationToken cancellationToken = default);
    Task SoftDelete(Guid id, CancellationToken cancellationToken = default);
    IDbContextTransaction BeginTransaction();
    DbSet<TEntity> GetDataSet();
}