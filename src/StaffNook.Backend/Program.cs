using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Newtonsoft.Json.Converters;
using Serilog;
using StaffNook.Backend.Filters;
using StaffNook.Infrastructure;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Extensions;
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
});
services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name!);
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddFluentValidationRulesToSwagger();

services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});

services.AddScoped<ILogger, ContextualLogger>();
services.AddScoped<ContextualLogger>();
services.AddScoped<GlobalExceptionFilter>();

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