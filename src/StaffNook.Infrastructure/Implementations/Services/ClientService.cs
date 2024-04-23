using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Client;
using StaffNook.Domain.Entities.Client;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task Create(CreateClientDto createClientDto, CancellationToken cancellationToken = default)
    {
        var client = _mapper.Map<CreateClientDto, ClientEntity>(createClientDto);
        await _clientRepository.Insert(client, cancellationToken);
    }

    public async Task Update(Guid id, UpdateClientDto updateClientDto, CancellationToken cancellationToken = default)
    {
        var entity = await _clientRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<ClientEntity>(id);
        }

        var result = _mapper.Map(updateClientDto, entity);

        await _clientRepository.Update(result, cancellationToken);
    }

    public async Task<ClientInfoDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _clientRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<ClientEntity>(id);
        }

        return _mapper.Map<ClientEntity, ClientInfoDto>(entity);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _clientRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<ClientEntity>(id);
        }

        await _clientRepository.SoftDelete(id, cancellationToken);
    }

    public async Task<IEnumerable<ClientInfoDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await _clientRepository.GetList(x => !x.IsArchived, cancellationToken);
        return _mapper.Map<IEnumerable<ClientEntity>, IEnumerable<ClientInfoDto>>(entities);
    }

    public async Task<AvailableValue[]> GetAvailableValues()
    {
        if (_currentUserService.User.IsAdmin)
        {
            return await _clientRepository.GetDataSet()
                .Where(x => !x.IsArchived)
                .Select(x => new AvailableValue(x.Id, x.Name))
                .ToArrayAsync();
        }

        return await _clientRepository.GetDataSet()
            .Include(x => x.Projects)
            .ThenInclude(x => x.ProjectEmployees)
            .Where(x => !x.IsArchived && 
                        x.Projects.Any(p=>p.ProjectEmployees
                            .Any(pe=>pe.UserId == _currentUserService.User.Id)))
            .Select(x => new AvailableValue(x.Id, x.Name))
            .ToArrayAsync();
    }

    public async Task<PaginationResult<ClientInfoDto>> GetByPageFilter(ClientPageFilter pageFilter = default,
        CancellationToken cancellationToken = default)
    {
        var query = _clientRepository.GetDataSet().Where(x => !x.IsArchived);

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<ClientInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<ClientEntity>, IEnumerable<ClientInfoDto>>(response),
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