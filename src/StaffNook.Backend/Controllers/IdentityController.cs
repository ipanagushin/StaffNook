using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffNook.Backend.Attributes;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Interfaces.Services.Identity;

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
    public async Task<LoginResponseDto> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        return await _identityService.Login(loginRequestDto);
    }

    /// <summary>
    /// Смена пароля пользователя
    /// </summary>
    /// <param name="changePasswordRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [Route("password/change")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        await _identityService.ChangePassword(changePasswordRequestDto);
    
        return Ok();
    }

    /// <summary>
    /// Получение информации о текущем пользователе
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("currentUser")]
    public async Task<CurrentUserResponseDto> CurrentUser()
    {
       return await _identityService.GetCurrentUserInfo();
    }
}