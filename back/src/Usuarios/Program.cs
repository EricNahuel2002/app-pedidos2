using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Usuarios.contexto;
using Usuarios.entidad;
using Usuarios.middleware;
using Usuarios.repositorio;
using Usuarios.servicio;


var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var serverVersion = new MySqlServerVersion(new Version(9, 5, 0));

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("ADVERTENCIA: Cadena de conexi�n 'DefaultConnection' no encontrada.");
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

app.UseMiddleware<ErrorHandlingMiddleware>();

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
            SeedRoles(dbContext);
            Console.WriteLine("Usuarios: Migraciones aplicadas con �xito.");
        }
        catch (Exception ex)
        {
            // Captura errores de conexi�n o migraci�n. 
            // Esto sucede a menudo si el contenedor MySQL a�n no est� listo.
            Console.WriteLine($"Usuarios: ERROR al aplicar migraciones: {ex.Message}");
            // La configuraci�n de RetryOnFailure en el AddDbContext ayuda a mitigar este error.
        }
    }
}

static void SeedRoles(UsuariosDbContext dbContext)
{
    var rolesExistentes = dbContext.Roles.Select(r => r.Nombre).ToList();
    var roles = new[] { "cliente", "repartidor", "administrador" };

    foreach (var nombre in roles)
    {
        if (!rolesExistentes.Contains(nombre))
        {
            dbContext.Roles.Add(new Rol { Nombre = nombre });
        }
    }

    dbContext.SaveChanges();
}