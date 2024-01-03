using StaffNook.Domain.Claims;

namespace StaffNook.Domain.Interfaces.Services.Identity;

public interface IClaimService
{
    Task<IEnumerable<ClaimsGroup>> GetAllGroupedClaims();
    Task<bool> ClaimExist(string name);
}