using Microsoft.AspNetCore.Mvc;
using StaffNook.Backend.Attributes;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.News;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/news")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    /// <summary>
    /// Создание новости
    /// </summary>
    /// <param name="createNewsDto"></param>
    /// <returns></returns>
    [HttpPost]
    [AuthClaim(ClaimList.CreateNews)]
    public async Task<IActionResult> Create([FromBody] CreateNewsDto createNewsDto)
    {
        await _newsService.Create(createNewsDto);
        return Ok();
    }

    /// <summary>
    /// Обновление новости
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateNewsDto"></param>
    /// <returns></returns>
    [HttpPut]
    [AuthClaim(ClaimList.UpdateNews)]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsDto updateNewsDto)
    {
        await _newsService.Update(id, updateNewsDto);
        return Ok();
    }

    /// <summary>
    /// Получение информации о новости
    /// </summary>
    /// <param name="newsId"></param>
    /// <returns></returns>
    [HttpGet("{newsId:guid}")]
    public async Task<NewsInfoDto> GetNewsById(Guid newsId)
    {
        return await _newsService.GetById(newsId);
    }

    /// <summary>
    /// Удаление новости
    /// </summary>
    /// <param name="newsId"></param>
    /// <returns></returns>
    [HttpDelete("{newsId:guid}")]
    [AuthClaim(ClaimList.DeleteNews)]
    public async Task<IActionResult> DeleteNewsById(Guid newsId)
    {
        await _newsService.Delete(newsId);
        return NoContent();
    }

    /// <summary>
    /// Получение новостей по фильтру с пагинацией
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("filter")]
    public async Task<PaginationResult<NewsInfoDto>> GetByPageFilter([FromBody] NewsPageFilter filter)
    {
        return await _newsService.GetByPageFilter(filter);
    }
}