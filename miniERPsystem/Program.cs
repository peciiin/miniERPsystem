using Microsoft.EntityFrameworkCore;
using miniERPsystem.Models;
using miniERPsystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MiniErpsystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductionService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<SellService>();
builder.Services.AddScoped<FinanceService>();
builder.Services.AddScoped<AutomaticOrderService>();

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
