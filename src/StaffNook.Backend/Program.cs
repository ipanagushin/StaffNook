using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using StaffNook.Backend.Filters;
using StaffNook.Infrastructure;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services 
AppConfiguration.Init(builder.Configuration);

var services = builder.Services;

services.ConfigureDbContext(builder.Configuration);
services.ConfigureDependencyContainer(builder.Configuration);
services.AddIdentity();
services.ConfigureIdentity();
services.ConfigureAuthentication(builder.Configuration);
services.AddControllers(options =>
{
    options.Filters.Add(typeof(RequestModelValidationFilter));
});
services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name!);
services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddFluentValidationRulesToSwagger();

services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});

services.AddValidatorsFromAssemblyContaining<ReflectionMarker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();