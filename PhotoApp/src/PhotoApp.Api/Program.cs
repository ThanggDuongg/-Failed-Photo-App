using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using PhotoApp.Api;
using PhotoApp.Api.Middlewares;
using PhotoApp.Core.Interfaces.Repositories;
using PhotoApp.Core.Interfaces.Services;
using PhotoApp.Core.Services;
using PhotoApp.Infrastructure.Context;
using PhotoApp.Infrastructure.Repositories;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddConsole();
builder.Logging.AddNLog();

// Add services to the container.

// Configure Cors
builder.Services.ConfigureCors();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Add Swagger
builder.Services.ConfigureSwagger();

// Configure dependency injection
builder.Services.ConfigureDependencyInjection(builder.Configuration);

// Config Auto Mapper    
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure All Url is lowcase
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Configure Health Check
builder.Services.ConfigureHealthChecks(builder.Configuration);

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, reloadOnChange: true);

IConfigurationRoot configurationRoot = configurationBuilder.Build();
LogManager.Configuration = new NLogLoggingConfiguration(configurationRoot.GetSection("NLog"));
Logger logger = LogManager.GetCurrentClassLogger();
//IApiVersionDescriptionProvider provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();


try
{
    logger.Info($"{ApiConstants.FriendlyServiceName} starts running...");
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // 
        configurationBuilder.AddJsonFile($"appsettings.{app.Environment.ToString()}.json", optional: false);

        app.UseDeveloperExceptionPage();
       /* app.UseHttpCodeAndLogMiddleware();*/

        // Configure Use Swagger {UI}
        
    }
    else
    {
        app.UseHttpCodeAndLogMiddleware();
        /*app.UseExceptionHandler("/Error");*/
        app.UseHsts();
    }

    app.ConfigureSwagger(builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>());

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    //HealthCheck Middleware
    app.MapHealthChecks("api/health", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHealthChecksUI(delegate (Options options)
    {
        options.UIPath = "/healthcheck-ui";
        options.AddCustomStylesheet("./HealthCheck/Custom.css");
    });

    app.Run();
    logger.Info($"{ApiConstants.FriendlyServiceName} is stopped");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
    throw;
}
finally
{
    LogManager.Shutdown();
}