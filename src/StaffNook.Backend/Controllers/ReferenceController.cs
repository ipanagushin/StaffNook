using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Reference;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/references")]
public class ReferenceController : ControllerBase
{
    private readonly IReferenceService _referenceService;

    public ReferenceController(IReferenceService referenceService)
    {
        _referenceService = referenceService;
    }
    
    [HttpGet("{type}")]
    public async Task<AvailableValue[]> GetReference([FromRoute] ReferenceType type)
    {
        return await _referenceService.GetAllReferencesByType(type);
    }

    [HttpPost("{type}")]
    public async Task<AvailableValue> AddReference([FromRoute] ReferenceType type, [FromBody] CreateReferenceDto createReferenceDto)
    {
        return await _referenceService.AddReference(type, createReferenceDto);
    }

    [HttpPut("{type}/{id:guid}")]
    public async Task<AvailableValue> UpdateReference([FromRoute] ReferenceType type, [FromRoute] Guid id, [FromBody] UpdateReferenceDto updateReferenceDto)
    {
        return await _referenceService.UpdateReference(type, id, updateReferenceDto);
    }
}