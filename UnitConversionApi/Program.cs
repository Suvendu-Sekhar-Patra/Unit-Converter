using UnitConversionApi.Core.Interfaces;
using UnitConversionApi.Middleware;
using UnitConversionApi.Services.Registry;
using UnitConversionApi.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddSingleton<IUnitRegistry, UnitRegistry>();
builder.Services.AddScoped<IConversionService, ConversionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Welcome to the Unit Conversion API! Navigate to /swagger to view the documentation.");

app.Run();
