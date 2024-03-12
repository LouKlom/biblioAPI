using Microsoft.EntityFrameworkCore;
using bibliotheque.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ClientContext>(opt =>
    opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AuteurContext>(opt =>
    opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<MediasContext>(opt =>
    opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ReservationContext>(opt =>
    opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
