using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using StaffNook.Backend.Filters;
using StaffNook.Domain.Interfaces.Commands;
using StaffNook.Infrastructure;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Extensions;
using StaffNook.Infrastructure.Implementations.Commands;
using StaffNook.Infrastructure.Logging;
using ILogger = StaffNook.Infrastructure.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services 
AppConfiguration.Init(builder.Configuration);

var services = builder.Services;

services.ConfigureDbContext(builder.Configuration);
services.ConfigureDependencyContainer(builder.Configuration);
services.ConfigureAuthentication(builder.Configuration);
services.AddControllers(options =>
    {
        options.Filters.Add(typeof(LoggingActionFilter));
        options.Filters.Add(typeof(GlobalExceptionFilter));
        options.Filters.Add(typeof(RequestModelValidationFilter));
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"
        });

        // options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
    });

services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name!);
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddFluentValidationRulesToSwagger();

services.AddFluentValidationAutoValidation(options => { options.DisableDataAnnotationsValidation = true; });

services.AddScoped<ILogger, ContextualLogger>();
services.AddScoped<ContextualLogger>();
services.AddScoped<GlobalExceptionFilter>();
services.AddScoped<IMoveAttachmentCommand, MoveAttachmentCommand>();

services.AddValidatorsFromAssemblyContaining<ReflectionMarker>();

services.AddCors(o =>
{
    o.AddPolicy("any", builder =>
    {
        builder
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var loggerFactory = app.Services.GetService<ILoggerFactory>()!;

loggerFactory.AddSerilog(DefaultLoggerFactory.LoggerFactory.Value, true);

app.UseHttpsRedirection();

app.UseCors("any");

app.UseAuthorization();

app.MapControllers();

app.Run();