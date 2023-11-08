namespace StaffNook.Domain.Configuration;

public class JwtConfiguration
{
    public string SecurityKey { get; set; }
    public int LifeTime { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}