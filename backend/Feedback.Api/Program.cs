using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        //policy.WithOrigins("https://feedback-k.netlify.app")
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Логируем все входящие запросы
app.Use((ctx, next) =>
{
    Console.WriteLine($"➡️  Incoming: {ctx.Request.Method} {ctx.Request.Path}");
    return next();
});

//app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Логируем все входящие запросы
app.Use((ctx, next) =>
{
    Console.WriteLine($"➡️  Incoming: {ctx.Request.Method} {ctx.Request.Path}");
    return next();
});

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();