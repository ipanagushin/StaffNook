using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.WorkingTime;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class WorkingTimeService : IWorkingTimeService
{
    private readonly IWorkingTimeRepository _workingTimeRepository;
    private readonly IMapper _mapper;

    public WorkingTimeService(IWorkingTimeRepository workingTimeRepository, IMapper mapper)
    {
        _workingTimeRepository = workingTimeRepository;
        _mapper = mapper;
    }


    public async Task Create(Guid userId, CreateWorkingTimeDto createWorkingTimeDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<CreateWorkingTimeDto, WorkingTimeEntity>(createWorkingTimeDto);
        entity.UserId = userId;

        await _workingTimeRepository.Insert(entity, cancellationToken);
    }

    public async Task Update(Guid id, UpdateWorkingTimeDto updateWorkingTimeDto, CancellationToken cancellationToken = default)
    {
        var entity = await _workingTimeRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<WorkingTimeEntity>(id);
        }

        var result = _mapper.Map(updateWorkingTimeDto, entity);

        await _workingTimeRepository.Update(result, cancellationToken);
    }

    public async Task<WorkingTimeInfoDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _workingTimeRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<WorkingTimeEntity>(id);
        }

        return _mapper.Map<WorkingTimeEntity, WorkingTimeInfoDto>(entity);
    }

    public async Task<PaginationResult<WorkingTimeInfoDto>> GetByPageFilter(WorkingTimePageFilter pageFilter = default, 
        CancellationToken cancellationToken = default)
    {
        var query = _workingTimeRepository.GetDataSet().Where(x => !x.IsArchived);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<WorkingTimeInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<WorkingTimeEntity>, IEnumerable<WorkingTimeInfoDto>>(response),
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