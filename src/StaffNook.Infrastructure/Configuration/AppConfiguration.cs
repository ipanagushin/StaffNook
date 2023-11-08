using Microsoft.Extensions.Configuration;
using StaffNook.Domain.Configuration;

namespace StaffNook.Infrastructure.Configuration;

public class AppConfiguration
{
    public static void Init(ConfigurationManager builderConfiguration)
    {
        JwtConfiguration = builderConfiguration.GetSection("JWT").Get<JwtConfiguration>();
    }
    
    public static JwtConfiguration JwtConfiguration { get; private set; }
}