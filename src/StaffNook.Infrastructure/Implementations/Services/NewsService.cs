using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.News;
using StaffNook.Domain.Entities.News;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public NewsService(INewsRepository newsRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task Create(CreateNewsDto createNewsDto, CancellationToken cancellationToken = default)
    {
        var newsEntity = _mapper.Map<CreateNewsDto, NewsEntity>(createNewsDto);
        await _newsRepository.Insert(newsEntity, cancellationToken);
    }

    public async Task Update(Guid id, UpdateNewsDto updateNewsDto, CancellationToken cancellationToken = default)
    {
        var newsEntity = await _newsRepository.GetById(id, cancellationToken);

        if (newsEntity is null)
        {
            throw NotFoundException.With<NewsEntity>(id);
        }

        var result = _mapper.Map(updateNewsDto, newsEntity);

        await _newsRepository.Update(result, cancellationToken);
    }

    public async Task<NewsInfoDto> GetById(Guid newsId, CancellationToken cancellationToken = default)
    {
        var newsEntity = await _newsRepository.GetById(newsId, cancellationToken);

        if (newsEntity is null)
        {
            throw NotFoundException.With<NewsEntity>(newsId);
        }

        return _mapper.Map<NewsEntity, NewsInfoDto>(newsEntity);
    }

    public async Task Delete(Guid newsId, CancellationToken cancellationToken = default)
    {
        var newsEntity = await _newsRepository.GetById(newsId, cancellationToken);

        if (newsEntity is null)
        {
            throw NotFoundException.With<NewsEntity>(newsId);
        }

        await _newsRepository.SoftDelete(newsId, cancellationToken);
    }

    public async Task<IEnumerable<NewsInfoDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var userEntities = await _newsRepository.GetList(x => !x.IsArchived, cancellationToken);
        return _mapper.Map<IEnumerable<NewsEntity>, IEnumerable<NewsInfoDto>>(userEntities);
    }

    public async Task<PaginationResult<NewsInfoDto>> GetByPageFilter(NewsPageFilter pageFilter = default,
        CancellationToken cancellationToken = default)
    {
        var query = _newsRepository.GetDataSet().Where(x => !x.IsArchived);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<NewsInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<NewsEntity>, IEnumerable<NewsInfoDto>>(response),
            PageInfo = new PageInfo
            {
                TotalCount = totalCount,
                CurrentPage = pageFilter.PageNumber,
                TotalPageCount = totalPages
            }
        };

        return paginationResult;
    }
}