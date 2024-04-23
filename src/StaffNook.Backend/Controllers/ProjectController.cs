using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Project;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    [HttpPost]
    // [AuthClaim(ClaimList.CreateClient)]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createDto)
    {
        await _projectService.CreateProject(createDto);
        return Ok();
    }
    
    /// <summary>
    /// Получение проектов по фильтру с пагинацией
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("filter")]
    public async Task<PaginationResult<ProjectInfoDto>> GetByPageFilter([FromBody] ProjectPageFilter filter)
    {
        return await _projectService.GetByPageFilter(filter);
    }
}