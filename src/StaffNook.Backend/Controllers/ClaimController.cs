using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Backend.Controllers;

[Route("api/v1/claim")]
public class ClaimController : ControllerBase
{
    private readonly IClaimService _claimService;

    public ClaimController(IClaimService claimService)
    {
        _claimService = claimService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ClaimsGroup>> GetGroupedClaims()
    {
        return await _claimService.GetAllGroupedClaims();
    }
}