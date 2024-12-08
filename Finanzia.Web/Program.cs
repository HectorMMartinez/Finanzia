using Microsoft.AspNetCore.Authentication.Cookies;
using Finanzia.Application;
using Finanzia.Application.Contract;
using Finanzia.Application.Services;
using Prestamo.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar la cadena de conexi�n desde el archivo de configuraci�n
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

// Registrar los servicios
builder.Services.AddSingleton<IMonedaService, MonedaService>();
builder.Services.AddSingleton<IClienteService, ClienteService>();
builder.Services.AddSingleton<IPrestamoService, PrestamoService>();
builder.Services.AddSingleton<IResumenService, ResumenService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();

// Configuraci�n de la autenticaci�n de cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true; // Extiende la expiraci�n si hay actividad
    });

// Configuraci�n de HttpClient para consumir API
builder.Services.AddHttpClient("PrestamoApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7291/"); // Cambia la URL seg�n tu configuraci�n
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Activar autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

