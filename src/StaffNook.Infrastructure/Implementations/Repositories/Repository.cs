using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;


//ToDo:: когда будет сервис пользователей, подумать где размещать заполнение основной информации из BaseEntity
// Было решено заполнять данные в репозитории с использованием CurrentUserIdentity сервиса, либо общего контекста вызова
//ToDo:: Добавить стандартное получение данных по фильтру с пагинацией
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly Context _context;
    private readonly ICurrentUserService _currentUserService;

    protected Repository(Context context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetDataSet().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<IEnumerable<TEntity>> GetList(Func<TEntity, bool>? filter = null, CancellationToken cancellationToken = default)
    {
        var query = filter != null ? 
            GetDataSet().Where(filter) : 
            GetDataSet();

        return await Task.FromResult(query.ToArray());
    }

    public async Task<TEntity> Insert(TEntity data, CancellationToken cancellationToken = default)
    {
        data.CreatedAt = DateTime.UtcNow;
        data.CreatorId = _currentUserService.User.Id;
        
        var result =  await GetDataSet().AddAsync(data, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<TEntity> Update(TEntity data, CancellationToken cancellationToken = default)
    {
        data.UpdatedAt = DateTime.UtcNow;
        var result = GetDataSet().Update(data);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task Delete(TEntity data, CancellationToken cancellationToken = default)
    {
        GetDataSet().Attach(data);
        _context.Remove(data);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDelete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetDataSet()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (entity is null)
        {
            throw new ArgumentException(nameof(id));
        }
        
        entity.IsArchived = true;

        await Update(entity, cancellationToken);
    }

    public DbSet<TEntity> GetDataSet()
    {
        return _context.Set<TEntity>();
    }
}