
using Microsoft.Extensions.Options;
using Utilities.Mail;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddScoped(typeof(IBaseData<>), typeof(ABaseData<>)); ACTIVENLA DESPUES DE HACER EL BUSINESS Y AÑADANLE LA DEL BUSINESS


// Add services to the container.
builder.Services.Configure<SwtpSettings>(builder.Configuration.GetSection("SwtpSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<SwtpSettings>>().Value);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
