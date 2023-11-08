using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos.Employee;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/identity")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="loginRequestDto">Модель авторизации пользователя</param>
    /// <returns>Результат авторизации</returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        var result = await _identityService.Login(loginRequestDto);

        return Ok(result);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        await _identityService.Create(createUserDto);

        return Ok();
    }
    
    
    [HttpPost]
    [Authorize]
    [Route("test")]
    public async Task<IActionResult> Test()
    {
        // await _identityService.Create();

        return Ok();
    }
}