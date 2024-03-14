using Microsoft.EntityFrameworkCore;
using bibliotheque.Models;
using System.Configuration;
using bibliotheque;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),

        opts => opts.MigrationsAssembly(typeof(ApiContext).Assembly.FullName));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoint();

app.Run();
