using Microsoft.EntityFrameworkCore;
using PhotoApp.Api;
using PhotoApp.Core.Interfaces.Repositories;
using PhotoApp.Core.Interfaces.Services;
using PhotoApp.Core.Services;
using PhotoApp.Infrastructure.Context;
using PhotoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Configure Use Swagger {UI}
    app.ConfigureSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();