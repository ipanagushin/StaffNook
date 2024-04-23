using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Commands;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHashService _hashService;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMoveAttachmentCommand _moveAttachmentCommand;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper, IHashService hashService,
        IRoleRepository roleRepository,
        ICurrentUserService currentUserService,
        IMoveAttachmentCommand moveAttachmentCommand)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _hashService = hashService;
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _moveAttachmentCommand = moveAttachmentCommand;
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

        var attachmentId = await _moveAttachmentCommand.GetOrUpdateAttachmentId(createUserDto.Attachment);
        userEntity.AttachmentId = attachmentId;

        await _userRepository.Insert(userEntity, cancellationToken);
    }

    public async Task Update(Guid id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        if (id != _currentUserService.User.Id && !_currentUserService.User.IsAdmin)
        {
            throw new ForbiddenException();
        }

        var userEntity = await _userRepository.GetById(id, cancellationToken);

        if (userEntity is null)
        {
            throw NotFoundException.With<UserEntity>(id);
        }

        var role = await _roleRepository.GetById(updateUserDto.RoleId, cancellationToken);
        if (role is null)
        {
            throw NotFoundException.With<RoleEntity>(updateUserDto.RoleId);
        }

        var attachmentId = await _moveAttachmentCommand.GetOrUpdateAttachmentId(updateUserDto.Attachment);

        var result = _mapper.Map(updateUserDto, userEntity);
        result.AttachmentId = attachmentId;

        await _userRepository.Update(result, cancellationToken);
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
        var userEntities = await _userRepository.GetList(x => !x.IsArchived, cancellationToken);
        return _mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserInfoDto>>(userEntities);
    }

    public async Task<PaginationResult<UserInfoDto>> GetAdminByPageFilter(UserPageFilter pageFilter = default,
        CancellationToken cancellationToken = default)
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

    public async Task<PaginationResult<ShortUserInfoDto>> GetByPageFilter(UserPageFilter pageFilter = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<UserEntity> query = _userRepository.GetDataSet();

        if (pageFilter.SpecialityId.HasValue)
        {
            query = query.Where(x => x.SpecialityId == pageFilter.SpecialityId.Value);
        }

        // xD
        if (!string.IsNullOrWhiteSpace(pageFilter.FullName))
        {
            var fullNameLower = pageFilter.FullName.ToLower();
            query = query.Where(x =>
                fullNameLower.Contains(x.FirstName.ToLower()) ||
                fullNameLower.Contains(x.LastName.ToLower()) ||
                fullNameLower.Contains(x.MiddleName.ToLower()) ||
                x.FirstName.ToLower().Contains(fullNameLower) ||
                x.LastName.ToLower().Contains(fullNameLower) ||
                x.MiddleName.ToLower().Contains(fullNameLower));
        }

        if (!string.IsNullOrWhiteSpace(pageFilter.PhoneNumber))
        {
            query = query.Where(x => x.PhoneNumber == pageFilter.PhoneNumber);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageFilter.PageSize);

        var response = await query
            .OrderByDescending(x => x.LastName)
            .Skip((pageFilter.PageNumber - 1) * pageFilter.PageSize)
            .Take(pageFilter.PageSize)
            .ToListAsync(cancellationToken);

        var paginationResult = new PaginationResult<ShortUserInfoDto>()
        {
            Items = _mapper.Map<IEnumerable<UserEntity>, IEnumerable<ShortUserInfoDto>>(response),
            PageInfo = new PageInfo
            {
                TotalCount = totalCount,
                CurrentPage = pageFilter.PageNumber,
                TotalPageCount = totalPages
            }
        };

        return paginationResult;
    }

    public async Task<AvailableValue[]> GetAvailableValues()
    {
       return await _userRepository.GetDataSet()
            .Where(x => !x.IsArchived)
            .Select(x => new AvailableValue(x.Id, x.FullName))
            .ToArrayAsync();
    }
}