using Microsoft.AspNetCore.Mvc;
using StaffNook.Backend.Attributes;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Client;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/client")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>
    /// Создание клиента
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    [HttpPost]
    [AuthClaim(ClaimList.CreateClient)]
    public async Task<IActionResult> Create([FromBody] CreateClientDto createDto)
    {
        await _clientService.Create(createDto);
        return Ok();
    }

    /// <summary>
    /// Обновление клиента
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    [HttpPut]
    [AuthClaim(ClaimList.UpdateClient)]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientDto updateDto)
    {
        await _clientService.Update(id, updateDto);
        return Ok();
    }

    /// <summary>
    /// Получение информации о клиенте
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ClientInfoDto> GetById(Guid id)
    {
        return await _clientService.GetById(id);
    }

    /// <summary>
    /// Удаление клиента
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [AuthClaim(ClaimList.DeleteClient)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        await _clientService.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Получение клиентов по фильтру с пагинацией
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("filter")]
    public async Task<PaginationResult<ClientInfoDto>> GetByPageFilter([FromBody] ClientPageFilter filter)
    {
        return await _clientService.GetByPageFilter(filter);
    }
    
    /// <summary>
    /// Получение клиентов для выпадающего списка
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("availableValues")]
    public async Task<AvailableValue[]> GetAvailableValues()
    {
        return await _clientService.GetAvailableValues();
    }
}