using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/role")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    /// <summary>
    /// Получение информации о роли по идентификатору
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet("{roleId:guid}")]
    public async Task<RoleInfoDto> GetRoleById(Guid roleId)
    {
        return await _roleService.GetById(roleId);
    }
    
    /// <summary>
    /// Получение списка всех ролей
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<RoleInfoDto>> GetAllRoles()
    {
        return await _roleService.GetAll();
    }
    
    /// <summary>
    /// Редактирование роли
    /// </summary>
    /// <param name="id"></param>
    /// <param name="editRoleDto"></param>
    [HttpPut("{id:guid}")]
    public async Task EditRole(Guid id, [FromBody] EditRoleDto editRoleDto)
    {
        await _roleService.Edit(id, editRoleDto);
    }
    
    /// <summary>
    /// Создание новой роли
    /// </summary>
    /// <param name="createModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RoleInfoDto> AddRole(EditRoleDto createModel)
    {
        return await _roleService.Create(createModel);
    }
}