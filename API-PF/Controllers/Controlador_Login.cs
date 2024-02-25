using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API_PF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Login : ControllerBase
    {
        private readonly Contexto contexto;

        public Controlador_Login(Contexto contexto)
        {
            this.contexto = contexto;
        }
        [HttpPost]
        public IActionResult IniciarSesion([FromBody] Usuario usuarioLogin)
        {
            try
            {       
                var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.email_usuario == usuarioLogin.email_usuario);
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var salt = config.GetSection("Salt");
                string stringSalt = salt["string"];
                usuarioLogin.passwd_usuario= Utils.Utils.HashPassword(usuarioLogin.passwd_usuario, stringSalt);
                if (usuarioExistente!=null)
                {
                    if (usuarioExistente.registrado == false)
                    {
                        Utils.Utils.Log("Login-Error confirmacion de registro");
                        return Conflict(new { mensaje = "No se ha confirmado el registro de usuario" });
                    }
                    else if (usuarioLogin.passwd_usuario== usuarioExistente.passwd_usuario)
                    {

                        // Contraseña correcta, procede con el inicio de sesión
                        Utils.Utils.Log("Un usuario ha iniciado sesión");
                        return Ok(new {usuario = usuarioExistente });
                    }
                    else
                    {
                        // Contraseña incorrecta
                        Utils.Utils.Log("Login-Contraseña incorrecta");
                        return Conflict(new { mensaje = "Contraseña incorrecta" });
                    }
                }
                else
                {
                    Utils.Utils.Log("Login-Email incorrecto");
                    return Conflict(new { mensaje = "Email no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new { Mensaje = "[ERROR-IniciarSesion([FromBody] Usuario usuarioLogin)]Error al iniciar sesión." });
            }
        }
        
    }
}
