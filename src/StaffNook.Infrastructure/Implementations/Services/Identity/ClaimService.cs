using StaffNook.Domain.Claims;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Infrastructure.Implementations.Services.Identity;

public class ClaimService : IClaimService
{
    public async Task<IEnumerable<ClaimsGroup>> GetAllGroupedClaims()
    {
        return await Task.FromResult(GroupedClaims.GetAllGroupedClaims());
    }

    public async Task<bool> ClaimExist(string name)
    {
        return await Task.FromResult(ClaimList.AllClaimsDictionary.ContainsKey(name));
    }
}