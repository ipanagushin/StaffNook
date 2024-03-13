using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.News;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface INewsService
{
    Task Create(CreateNewsDto createNewsDto, CancellationToken cancellationToken = default);
    Task Update(Guid id, UpdateNewsDto updateNewsDto, CancellationToken cancellationToken = default);
    Task<NewsInfoDto> GetById(Guid newsId, CancellationToken cancellationToken = default);
    Task Delete(Guid newsId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NewsInfoDto>> GetAll(CancellationToken cancellationToken = default);
    Task<PaginationResult<NewsInfoDto>> GetByPageFilter(NewsPageFilter pageFilter = default, CancellationToken cancellationToken = default);
}