using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);


// carga base (opcional)
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Leer y merge manual de archivos Ocelot que contienen "Routes"
var merged = new JObject();
var allRoutes = new JArray();

var files = new[] { "ocelot.json", "ocelot.usuarios.json", "ocelot.menus.json", "ocelot.ordenes.json" };
foreach (var f in files)
{
    if (!File.Exists(f)) continue;
    var j = JObject.Parse(File.ReadAllText(f));
    var routes = j["Routes"] as JArray;
    if (routes != null)
        foreach (var r in routes)
            allRoutes.Add(r);
    // si hay GlobalConfiguration y no existe en merged a��delo
    if (merged["GlobalConfiguration"] == null && j["GlobalConfiguration"] != null)
        merged["GlobalConfiguration"] = j["GlobalConfiguration"];
}
merged["Routes"] = allRoutes;

// A�adimos la configuraci�n mergeada a memoria para que Ocelot la use
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
              .AllowAnyHeader()
              .AllowAnyMethod();
        });
});

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();
app.UseRouting();

app.UseCors("AllowAngular");


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


await app.UseOcelot();
await app.RunAsync();