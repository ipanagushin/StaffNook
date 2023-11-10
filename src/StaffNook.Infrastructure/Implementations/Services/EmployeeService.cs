using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Employee;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<UserEntity> _userManager;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IMapper mapper,
        UserManager<UserEntity> userManager)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<EmployeeInfoDto> GetById(Guid id)
    {
        var entity = await _employeeRepository.GetById(id);
        return _mapper.Map<EmployeeInfoDto>(entity);
    }

    public async Task Create(CreateEmployeeDto dto)
    {
        var userEntity = await _userManager.FindByIdAsync(dto.UserId.ToString());

        if (userEntity is null)
        {
            throw NotFoundException.With<UserEntity>(dto.UserId);
        }

        var userRoles = await _userManager.GetRolesAsync(userEntity);

        if (userRoles.FirstOrDefault(x => x == IdentityRoles.User) is null)
        {
            throw new BusinessException($"Пользователь с идентификатором {dto.UserId} не имеет роль {IdentityRoles.User}");
        }

        var entity = _mapper.Map<EmployeeEntity>(dto);
        await _employeeRepository.Insert(entity);
    }
}