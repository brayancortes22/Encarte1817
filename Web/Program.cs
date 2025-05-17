using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities.Interfaces;
using Utilities.Jwt;


var builder = WebApplication.CreateBuilder(args);


// Registrar el servicio IJwtGenerator con su implementación GenerateTokenJwt
builder.Services.AddScoped<IJwtGenerator, GenerateTokenJwt>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // si estás usando Swashbuckle u OpenAPI

// 🔐 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
