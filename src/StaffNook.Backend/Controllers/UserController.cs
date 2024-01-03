using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="createUserDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
    {
        await _userService.Create(createUserDto);
        return Ok();
    }
    
    /// <summary>
    /// Получение информации о пользователе
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId:guid}")]
    public async Task<UserInfoDto> GetUser(Guid userId)
    {
        return await _userService.GetById(userId);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<UserInfoDto>> GetUsers()
    {
        return await _userService.GetAll();
    }
    
    [HttpPost]
    [Route("filter")]
    public async Task<PaginationResult<UserInfoDto>> GetByPageFilter()
    {
        return await _userService.GetByPageFilter(new UserPageFilter()
        {
            PageNumber = 1,
            PageSize = 10
        });
    }
}