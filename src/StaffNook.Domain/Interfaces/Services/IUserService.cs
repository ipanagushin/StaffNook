﻿using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface IUserService
{
    Task Create(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task Update(Guid id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
    Task<UserInfoDto> GetById(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserInfoDto>> GetAll(CancellationToken cancellationToken = default);
    Task<PaginationResult<UserInfoDto>> GetAdminByPageFilter(UserPageFilter pageFilter = default, CancellationToken cancellationToken = default);
    Task<PaginationResult<ShortUserInfoDto>> GetByPageFilter(UserPageFilter pageFilter = default, CancellationToken cancellationToken = default);
    Task<AvailableValue[]> GetAvailableValues();
}