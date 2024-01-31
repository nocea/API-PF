using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;

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
                if (usuarioExistente!=null)
                {
                    if (BCrypt.Net.BCrypt.Verify(usuarioLogin.passwd_usuario, usuarioExistente.passwd_usuario))
                    {
                        // Contraseña correcta, procede con el inicio de sesión
                        return Ok(new { mensaje = "Inicio de sesión exitoso" });
                    }
                    else
                    {
                        // Contraseña incorrecta
                        return Conflict(new { mensaje = "Contraseña incorrecta" });
                    }
                }
                else
                {
                    return Conflict(new { mensaje = "Email no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error al iniciar sesión.", Error = ex.Message });
            }
        }
        
    }
}
