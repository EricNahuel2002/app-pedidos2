using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Usuarios.contexto;
using Usuarios.repositorio;
using Usuarios.servicio;


var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(9, 5, 0));

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("ADVERTENCIA: Cadena de conexión 'DefaultConnection' no encontrada.");
}

builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseMySql(
        connectionString,serverVersion,
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    )
);

builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<IUsuariosServicio, UsuariosServicio>();


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


app.Run();

static void ApplyMigrations(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();

        try
        {
            Console.WriteLine("Usuarios: Aplicando migraciones...");
            dbContext.Database.Migrate();
            Console.WriteLine("Usuarios: Migraciones aplicadas con éxito.");
        }
        catch (Exception ex)
        {
            // Captura errores de conexión o migración. 
            // Esto sucede a menudo si el contenedor MySQL aún no está listo.
            Console.WriteLine($"Usuarios: ERROR al aplicar migraciones: {ex.Message}");
            // La configuración de RetryOnFailure en el AddDbContext ayuda a mitigar este error.
        }
    }
}