using System.Text;
using Entity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearior;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Middleware;
using Web.ServiceExtension;
using Business.Interfaces;
using Business.Implements;
using Business.Services;
using Data.Interfaces;
using Data.Implements;
using Web.Services;
using Entity.Dtos.RolDTO;
using Entity.Dtos.RolUserDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Text;
using Entity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearior;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Middleware;
using Web.ServiceExtension;
using Business.Interfaces;
using Business.Implements;
using Business.Services;
using Data.Interfaces;
using Data.Implements;
using Web.Services;
using Entity.Dtos.RolDTO;
using Entity.Dtos.RolUserDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Add services to the container
builder.Services.AddControllers();

// Registro de servicios genéricos
builder.Services.AddScoped(typeof(IGenericData<>), typeof(GenericData<>));
builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));

// Registro de servicios específicos para User
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

// Registro de servicios específicos para Role
builder.Services.AddScoped<IRolData, RolData>();
builder.Services.AddScoped<IRolBusiness, RolBusiness>();

// Registro de servicios específicos para RoleUser
builder.Services.AddScoped<IRolUserData, RolUserData>();
builder.Services.AddScoped<IRoleUserBusiness, RoleUserBusiness>();

// Registro de servicios de autenticación
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuración de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add application services (validators, CORS, etc.)
builder.Services.AddApplicationServices(builder.Configuration);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de servicios gen�ricos
builder.Services.AddScoped(typeof(IGenericData<>), typeof(GenericData<>));
builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));

// Registro de servicios espec�ficos para User
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

// Registro de servicios espec�ficos para Role
builder.Services.AddScoped<IRolData, RolData>();
builder.Services.AddScoped<IRolBusiness, RolBusiness>();

// Registro de servicios espec�ficos para RoleUser
builder.Services.AddScoped<IRolUserData, RolUserData>();
builder.Services.AddScoped<IRoleUserBusiness, RoleUserBusiness>();

// Registro de servicios de autenticaci�n
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuraci�n de autenticaci�n JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add application services (validators, CORS, etc.)
builder.Services.AddApplicationServices(builder.Configuration);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gestión v1");
        c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la raíz
    });
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gesti�n v1");
        c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la ra�z
    });
}

// Middleware personalizado para manejo de excepciones
app.UseCustomExceptionHandler();

// Habilitar CORS
app.UseCors("AllowSpecificOrigins");

// Middleware personalizado para manejo de excepciones
app.UseCustomExceptionHandler();

// Habilitar CORS
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

// Agregar autenticación
app.UseAuthentication();
// Agregar autenticaci�n
app.UseAuthentication();
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

// Asegurar que la base de datos est� creada y aplicar migraciones pendientes
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
        logger.LogError(ex, "Ocurri� un error durante la migraci�n de la base de datos.");
    }
}

app.Run();