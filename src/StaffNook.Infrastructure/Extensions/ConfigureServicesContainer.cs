using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StaffNook.Domain.Configuration;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Commands;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Handlers;
using StaffNook.Infrastructure.Implementations.Commands;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Extensions;

public static class ConfigureServicesContainer
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("StaffNook.Infrastructure")
            ).UseLazyLoadingProxies();
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("JWT").Get<JwtConfiguration>();

        if (jwtConfiguration is null)
        {
            throw new Exception("JWT configuration is missing");
        }

        services.AddAuthentication("StaffNook")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = jwtConfiguration.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecurityKey))
                };
            }).AddScheme<AppAuthenticationSchemeOptions, AuthHandler>("StaffNook", _ => {});;
    }

    public static void ConfigureDependencyContainer(this IServiceCollection services, IConfiguration configuration)
    {
        var appRepositories = typeof(ReflectionMarker).Assembly.GetTypes()
            .Where(s => s.Name.EndsWith("Repository") && s.IsInterface == false).ToList();
        
        foreach (var appRepository in appRepositories)
        {
            var interfaceType = appRepository.GetInterfaces().FirstOrDefault(x => x.Name.EndsWith("Repository"));
            if (interfaceType is not null)
            {
                services.Add(new ServiceDescriptor(interfaceType, appRepository, ServiceLifetime.Scoped));
            }
        }

        var appServices = typeof(ReflectionMarker).Assembly.GetTypes()
            .Where(s => s.Name.EndsWith("Service") && s.IsInterface == false).ToList();
        foreach (var appService in appServices)
        {
            var interfaceType = appService.GetInterfaces().FirstOrDefault(x => x.Name.EndsWith("Service"));

            if (interfaceType is not null)
            {
                services.Add(new ServiceDescriptor(interfaceType, appService, ServiceLifetime.Scoped));
            }
        }

        // services.AddScoped<IMoveAttachmentCommand, MoveAttachmentCommand>();
    }

    public static void ConfigureSwagger(this IServiceCollection services, string assemblyName)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "StaffNook", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer {token}')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{assemblyName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}