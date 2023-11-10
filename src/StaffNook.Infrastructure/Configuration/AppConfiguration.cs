#nullable disable
using Microsoft.Extensions.Configuration;
using StaffNook.Domain.Configuration;
using StaffNook.Infrastructure.Logging;

namespace StaffNook.Infrastructure.Configuration;

public class AppConfiguration
{
    public static void Init(ConfigurationManager builderConfiguration)
    {
        JwtConfiguration = builderConfiguration.GetSection("JWT").Get<JwtConfiguration>();
        LoggingConfiguration = builderConfiguration.GetSection("Logging").Get<LoggingConfiguration>();
    }
    
    public static JwtConfiguration JwtConfiguration { get; private set; }
    public static LoggingConfiguration LoggingConfiguration { get; private set; }
}