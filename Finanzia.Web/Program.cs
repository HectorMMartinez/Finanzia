using Microsoft.AspNetCore.Authentication.Cookies;
using Finanzia.Application;
using Finanzia.Application.Contract;
using Finanzia.Application.Services;
using Prestamo.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar la cadena de conexión desde el archivo de configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

// Registrar los servicios
builder.Services.AddSingleton<IMonedaService, MonedaService>();
builder.Services.AddSingleton<IClienteService, ClienteService>();
builder.Services.AddSingleton<IPrestamoService, PrestamoService>();
builder.Services.AddSingleton<IResumenService, ResumenService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();

// Configuración de la autenticación de cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true; // Extiende la expiración si hay actividad
    });

// Configuración de HttpClient para consumir API
builder.Services.AddHttpClient("PrestamoApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7291/"); // Cambia la URL según tu configuración
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

// Activar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas del controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

