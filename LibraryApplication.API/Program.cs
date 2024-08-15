using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;
using SimpleLibraryAPI.Services;
using SimpleLibraryV2.Services;
using SimpleLibraryV2.NewFolder;
using SimpleLibraryV2.Interfaces;
using LibraryApplication.Infrastructure.Repositories;
using LibraryApplication.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<BookManager>();

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
