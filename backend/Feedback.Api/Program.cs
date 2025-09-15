using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("https://feedback-k.netlify.app")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Подключаем ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Активация CORS
app.UseCors("AllowAngular");

// Обязательные middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Важно: чтобы работали контроллеры

app.Run();