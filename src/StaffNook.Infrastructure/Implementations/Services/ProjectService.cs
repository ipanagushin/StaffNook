using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Project;
using StaffNook.Domain.Entities.Project;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Repositories.Project;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectContactsRepository _projectContactsRepository;
    private readonly IProjectRoleRepository _projectRoleRepository;
    private readonly IMapper _mapper;
    

    public ProjectService(
        IProjectRepository projectRepository, 
        IProjectContactsRepository projectContactsRepository, 
        IProjectRoleRepository projectRoleRepository, 
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _projectContactsRepository = projectContactsRepository;
        _projectRoleRepository = projectRoleRepository;
        _mapper = mapper;
    }

    public async Task CreateProject(CreateProjectDto createProjectDto, CancellationToken cancellationToken = default)
    {
        await using var transaction = _projectRepository.BeginTransaction();
        
        try
        {
            var projectEntity = _mapper.Map<CreateProjectDto, ProjectEntity>(createProjectDto);

            projectEntity = await _projectRepository.Insert(projectEntity, cancellationToken);

            var contacts = createProjectDto.Contacts.Select(contact =>
            {
                contact.ProjectId = projectEntity.Id;
                return contact;
            });
            
            var projectContacts =
                _mapper.Map<IEnumerable<ProjectContactDto>, IEnumerable<ProjectContactsEntity>>(contacts).ToArray();

            await _projectContactsRepository.InsertMany(projectContacts, cancellationToken);

            var roles = createProjectDto.Roles.Select(contact =>
            {
                contact.ProjectId = projectEntity.Id;
                return contact;
            });
            
            var projectRoles = 
                _mapper.Map<IEnumerable<ProjectRoleDto>, IEnumerable<ProjectRoleEntity>>(roles).ToArray();

            await _projectRoleRepository.InsertMany(projectRoles, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new BusinessException("Произошла ошибка при создании проекта");
        }
    }

    public Task UpdateProject(Guid id, UpdateProjectDto updateProjectDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginationResult<ProjectInfoDto>> GetByPageFilter(ProjectPageFilter pageFilter = default, 
        CancellationToken cancellationToken = default)
    {
        var query = _projectRepository.GetDataSet().Where(x => !x.IsArchived);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<ProjectInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<ProjectEntity>, IEnumerable<ProjectInfoDto>>(response),
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