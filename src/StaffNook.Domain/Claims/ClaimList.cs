using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StaffNook.Domain.Claims;

public static class ClaimList
{
    [Display(Name = "Создание пользователя")]
    public const string CreateUser = nameof(CreateUser);

    [Display(Name = "Обновление пользователя")]
    public const string UpdateUser = nameof(UpdateUser);
    
    public static Dictionary<string, string> AllClaimsDictionary = typeof(ClaimList).GetFields()
        .ToDictionary(x => x.Name, x => x.GetCustomAttribute<DisplayAttribute>()?.Name);

    public static string[] AllClaims = typeof(ClaimList).GetFields().Select(x => x.Name).ToArray();
}

public static class GroupedClaims
{
    private static string[] UserClaims = new[]
    {
        ClaimList.CreateUser,
        ClaimList.UpdateUser
    };
    
    public static IEnumerable<ClaimsGroup> GetAllGroupedClaims()
    {
        return new[]
        {
            new ClaimsGroup("Пользователи", GetClaimDescriptionByKeys(UserClaims))
        };
    }
    
    private static Dictionary<string, string> GetClaimDescriptionByKeys(params string[] claimKeys)
    {
        return claimKeys.Select(claimKey =>
        {
            var descriptionKey = ClaimList.AllClaimsDictionary.TryGetValue(claimKey, out var descClaim)
                ? descClaim
                : claimKey;

            return new KeyValuePair<string, string>(claimKey, descriptionKey);
        }).ToDictionary(x => x.Key, q => q.Value);
    }
}

public class ClaimsGroup
{
    public string GroupName { get; set; }
    public Dictionary<string, string> Claims { get; set; }

    public ClaimsGroup(string groupName, Dictionary<string, string> claims)
    {
        GroupName = groupName;
        Claims = claims;
    }
}