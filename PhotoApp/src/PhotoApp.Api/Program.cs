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
builder.Services.AddSwaggerGen();

// Configure dependency injection
builder.Services.ConfigureDependencyInjection(builder.Configuration);

// Config Auto Mapper    
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();