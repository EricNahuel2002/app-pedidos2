using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var merged = new JObject();
var allRoutes = new JArray();

var files = new[] { "ocelot.json", "ocelot.usuarios.json", "ocelot.menus.json", "ocelot.ordenes.json", "ocelot.auth.json" };
foreach (var f in files)
{
    if (!File.Exists(f)) continue;
    var j = JObject.Parse(File.ReadAllText(f));
    var routes = j["Routes"] as JArray;
    if (routes != null)
        foreach (var r in routes)
            allRoutes.Add(r);
    if (merged["GlobalConfiguration"] == null && j["GlobalConfiguration"] != null)
        merged["GlobalConfiguration"] = j["GlobalConfiguration"];
}
merged["Routes"] = allRoutes;

var memStream = new MemoryStream();
var sw = new StreamWriter(memStream);
sw.Write(merged.ToString());
sw.Flush();
memStream.Position = 0;
builder.Configuration.AddJsonStream(memStream);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
        });
});

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

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

var app = builder.Build();
app.UseRouting();

app.UseCors("AllowAngular");


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
await app.RunAsync();

public partial class Program { }