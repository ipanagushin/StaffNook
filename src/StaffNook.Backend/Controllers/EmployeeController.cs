using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Dtos.Employee;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto createEmployeeDto)
    {
        await _employeeService.Create(createEmployeeDto);
        return Ok();
    }
}