using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Finanzia.Domain.DTOs;
using Finanzia.Application.Contract;

namespace Finanzia.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string correo,string clave)
        {
            if (correo == null || clave == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            UsuarioDTO usuario_encontrado = new UsuarioDTO();
            usuario_encontrado = await _usuarioService.Login(correo,clave);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto),
                    new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role,"Administrador")
                };


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
            return RedirectToAction("Index", "Home");
        }
    }
}
