using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Web.Middleware;
using Web.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add application services (business, data, validators)
builder.Services.AddApplicationServices(builder.Configuration);

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gestión v1");
        c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la raíz
    });
}

// Middleware personalizado para manejo de excepciones
app.UseCustomExceptionHandler();

// Habilitar CORS
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Asegurar que la base de datos esté creada y aplicar migraciones pendientes
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        // Verifica si la base de datos existe y la crea si no
        if (dbContext.Database.EnsureCreated())
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Base de datos creada exitosamente.");
        }

        // Aplica las migraciones pendientes
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Migraciones aplicadas exitosamente.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la migración de la base de datos.");
    }
}

app.Run();