using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Ordenes.contexto;
using Ordenes.repositorio;
using Ordenes.servicio;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var jwtKey = configuration["Jwt:Key"];
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "auth-api",
            ValidAudience = "api-gateway",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(9, 5, 0));



if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("ADVERTENCIA: Cadena de conexión 'DefaultConnection' no encontrada.");
}

builder.Services.AddDbContext<OrdenesDbContext>(options =>
    options.UseMySql(
        connectionString, serverVersion,
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    )
);

builder.Services.AddScoped<IOrdenesRepositorio, OrdenesRepositorio>();
builder.Services.AddScoped<IOrdenesServicio, OrdenesServicio>();

builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://apigateway:5000/");
});

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapControllers();
ApplyMigrations(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();

static void ApplyMigrations(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        // NOTA: Se utiliza OrdenesDbContext para este microservicio
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdenesDbContext>();

        try
        {
            Console.WriteLine("Ordenes: Aplicando migraciones...");
            // Este método crea la base de datos si no existe y aplica todas las migraciones pendientes.
            dbContext.Database.Migrate();
            Console.WriteLine("Ordenes: Migraciones aplicadas con éxito.");
        }
        catch (Exception ex)
        {
            // Captura errores de conexión o migración. 
            Console.WriteLine($"Ordenes: ERROR al aplicar migraciones: {ex.Message}");
            // La configuración de RetryOnFailure en el AddDbContext ayuda a mitigar este error.
        }
    }
}