using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos.WorkingTime;
using StaffNook.Domain.Filters;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/working-time")]
public class WorkingTimeController : ControllerBase
{
    private readonly IWorkingTimeService _workingTimeService;
    private readonly ICurrentUserService _currentUserService;

    public WorkingTimeController(IWorkingTimeService workingTimeService, ICurrentUserService currentUserService)
    {
        _workingTimeService = workingTimeService;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkingTimeDto createWorkingTimeDto)
    {
        var userId = _currentUserService.User.Id;
        await _workingTimeService.Create(userId, createWorkingTimeDto);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkingTimeDto updateWorkingTimeDto)
    {
        await _workingTimeService.Update(id, updateWorkingTimeDto);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var workingTime = await _workingTimeService.GetById(id);
        if (workingTime == null)
            return NotFound();

        return Ok(workingTime);
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetByPageFilter([FromBody] WorkingTimePageFilter pageFilter)
    {
        var workingTimes = await _workingTimeService.GetByPageFilter(pageFilter);
        return Ok(workingTimes);
    }
}