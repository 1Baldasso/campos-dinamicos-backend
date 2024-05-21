using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(op => op.AddDefaultPolicy(pol =>
                            pol.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()));

builder.Services.AddDbContext<PocContext>(op =>
    op.UseInMemoryDatabase("TesteCamposDinamicos")
    .LogTo(Console.WriteLine, categories: [DbLoggerCategory.Database.Transaction.Name, DbLoggerCategory.Database.Name], LogLevel.Trace, DbContextLoggerOptions.Level));

builder.Services.AddScoped<IDatabaseProvider, DatabasePopulator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();