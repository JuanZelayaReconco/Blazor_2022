using Blazor.Data;
using Datos.Interfaces;
using Datos.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Blazor.Controllers
{
    public class LoginController : Controller
    {
        private readonly MySQLConfiguracion _configuracion;
        private ILoginRepositorio _loginRepositorio;
        private IUsuarioRepositorio _usuarioRepositorio;
        public LoginController(MySQLConfiguracion configuracion)
        {
            _configuracion = configuracion;
            _loginRepositorio = new LoginRepositorio(configuracion.CadenaConexion);
            _usuarioRepositorio = new UsuarioRepositorio(configuracion.CadenaConexion);
        }

        [HttpPost("/account/login")]
        public async Task<IActionResult> Login(Login login)
        {
            string rol = string.Empty;
            try
            {
                bool usuarioValido = await _loginRepositorio.ValidarUsuario(login);
                if (usuarioValido)
                {
                    Usuario user = await _usuarioRepositorio.GetPorCodigo(login.Codigo);
                    if (user.EstaActivo)
                    {
                        rol = user.Rol;

                        //Añadimos los claums del usuario y su rol.
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.Codigo),
                            new Claim(ClaimTypes.Role, rol)
                        };

                        //Crear el Clain principal.
                        var clainsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //"var" es lo mismo que poner "ClaimsIdentity", var detecta el tipo de la variable.
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(clainsIdentity);

                        //Generar cookies.
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties {IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddHours(2)});
                    }
                    else
                    {
                        return LocalRedirect("/login/{El usuario está inactivo}");
                    }
                }
                else
                {
                    return LocalRedirect("/login/{Datos del usuario invalido}");
                }
            }
            catch (Exception)
            {
            }

            return LocalRedirect("/"); //Página principal.
        }

        [HttpGet("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/login");
        }
    }
}
