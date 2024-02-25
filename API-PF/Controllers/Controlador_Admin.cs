using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
namespace API_PF.Controllers
{   /// <summary>
    /// Controlador para la administración de la aplicación
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Admin : Controller
    {
        private readonly Contexto contexto;//instancio el contexto para poder usarlo

        public Controlador_Admin(Contexto contexto)
        {
            this.contexto = contexto;
        }


        [HttpGet("ObtenerUsuarioNombre/{nombreUsuario}")]
        public ActionResult<Usuario> ObtenerUsuarioNombre(string nombreUsuario)
        {
            try
            {
                var usuario = contexto.usuarios.FirstOrDefault(u => u.nombreCompleto_usuario == nombreUsuario);
                if (usuario == null)
                {
                    return Conflict(new { mensaje = "[ERROR-ObtenerUsuarioNombre(string nombreUsuario)]No se ha encontrado el usuario" });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-ObtenerUsuarioNombre(string nombreUsuario)]No se ha podido obtener el usuario" });
            }
        }


    }
}


