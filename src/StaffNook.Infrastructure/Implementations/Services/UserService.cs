using System.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Converters;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHashService _hashService;
    private readonly IRoleRepository _roleRepository;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper, IHashService hashService, 
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _hashService = hashService;
        _roleRepository = roleRepository;
    }

    public async Task Create(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var role = await _roleRepository.GetById(createUserDto.RoleId, cancellationToken);
        if (role is null)
        {
            throw NotFoundException.With<RoleEntity>(createUserDto.RoleId);
        }
        
        var (hash, salt) = _hashService.GenerateHash(createUserDto.Password);

        var userEntity = _mapper.Map<CreateUserDto, UserEntity>(createUserDto);
        userEntity.Hash = hash;
        userEntity.Salt = salt;
        
        // ToDo:: вынести в репозиторий
        userEntity.CreateDate = DateTime.UtcNow;
        userEntity.UpdateDate = DateTime.UtcNow;

       await _userRepository.Insert(userEntity, cancellationToken);
    }

    public async Task<UserInfoDto> GetById(Guid userId, CancellationToken cancellationToken = default)
    {
       var userEntity = await _userRepository.GetById(userId, cancellationToken);

       if (userEntity is null)
       {
           throw NotFoundException.With<UserEntity>(userId);
       }

       return _mapper.Map<UserEntity, UserInfoDto>(userEntity);
    }

    public async Task<IEnumerable<UserInfoDto>> GetAll(CancellationToken cancellationToken = default)
    {
        var userEntities = await _userRepository.GetList(cancellationToken);
        return _mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserInfoDto>>(userEntities);
    }

    public async Task<PaginationResult<UserInfoDto>> GetByPageFilter(UserPageFilter pageFilter = default, CancellationToken cancellationToken = default)
    {
        var query = _userRepository.GetDataSet();
        //todo process filter

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.LastName)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<UserInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserInfoDto>>(response),
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