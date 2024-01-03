using AutoMapper;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services.Identity;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    public async Task<RoleInfoDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var roleEntity = await _roleRepository.GetById(id, cancellationToken);
        if (roleEntity is null)
        {
            throw NotFoundException.With<RoleEntity>(id);
        }

        return _mapper.Map<RoleEntity, RoleInfoDto>(roleEntity);
    }

    public async Task<IEnumerable<RoleInfoDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var roleEntities = await _roleRepository.GetList(cancellationToken);
        return _mapper.Map<IEnumerable<RoleEntity>, IEnumerable<RoleInfoDto>>(roleEntities);
    }

    public async Task Edit(Guid id, EditRoleDto infoDto, CancellationToken cancellationToken = default)
    {
        var roleEntity = await _roleRepository.GetById(id, cancellationToken);
        if (roleEntity is null)
        {
            throw NotFoundException.With<RoleEntity>(id);
        }
        
        throw new NotImplementedException();
    }

    public async Task<RoleInfoDto> Create(EditRoleDto dto, CancellationToken cancellationToken = default)
    {
        var roleEntity = _mapper.Map<EditRoleDto, RoleEntity>(dto);
        var newEntity = await _roleRepository.Insert(roleEntity, cancellationToken);
        return _mapper.Map<RoleEntity, RoleInfoDto>(newEntity);
    }
}