using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NayeliApi.Core.Interfaces;
using NayeliApi.Core.Services;
using NayeliApi.Infrastructure.Data;
using NayeliApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuramos la conexión a la base de datos SQLite
builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Agregamos nuestras Interfaces y Servicios
builder.Services.AddScoped<ICuentaBancariaRepository, CuentaBancariaRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICuentaBancariaService, CuentaBancariaService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     // Configuramos Swagger para que la UI esté en la raíz
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NayeliApi v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
