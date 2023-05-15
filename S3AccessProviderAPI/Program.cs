
using Microsoft.EntityFrameworkCore;
using S3AccessProviderAPI.Contracts;
using S3AccessProviderAPI.Models;
using S3AccessProviderAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FileDbContext>(options => options.UseSqlServer(connectionStrings));
builder.Services.AddSingleton<IS3HandlerService, AmazonS3HandlerService>();

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
