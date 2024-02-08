using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Gestion : Controller
    {
        private readonly Contexto contexto;

        public Controlador_Gestion(Contexto contexto)
        {
            this.contexto = contexto;
        }
        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            
            var usuarios = contexto.usuarios.ToList();

            
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public ActionResult<Usuario> ObtenerUsuario(int id)
        {
            var usuario = contexto.usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound(); 
            }
            return usuario; 
        }
        [HttpPost("EditarUsuario")]
        public IActionResult EditarUsuario([FromBody] Usuario usuario)
        {
            var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.id_usuario == usuario.id_usuario);
            
            if (usuarioExistente != null)
            {
                usuarioExistente.alias_usuario = usuario.alias_usuario;
                usuarioExistente.email_usuario = usuario.email_usuario;
                usuarioExistente.movil_usuario = usuario.movil_usuario;
                usuarioExistente.nombreCompleto_usuario=usuario.nombreCompleto_usuario;
                contexto.SaveChanges();
                return Ok(new { mensaje="Se ha cambiado el usuario"});
            }
            else
            {
                return Conflict(new {mensaje="No se ha encontrado el usuario en la BBDD"});
            }
        }
        [HttpDelete("EliminarUsuario/{id_usuario}")]
        public IActionResult EliminarUsuario(int id_usuario)
        {   
            var usuario = contexto.usuarios.Find(id_usuario);
            
            if (usuario == null||usuario.rol_usuario=="ADMIN")
            {
                return NotFound(new { mensaje = "El usuario no fue encontrado o no se puede eliminar" });
            }
            contexto.usuarios.Remove(usuario);
            contexto.SaveChanges();
            return Ok(new { mensaje = "Usuario eliminado exitosamente." });
        }
        [HttpPost("{id}")]
        public IActionResult BorrarUsuario(string id_usuario)
        {
            var usuarioBorrar = contexto.usuarios.Find(id_usuario);
            if (usuarioBorrar != null)
            {
                contexto.usuarios.Remove(usuarioBorrar);
                contexto.SaveChanges();
                return Ok(new { mensaje = "Se ha borrado el usuario" });
            }
            else
            {
                return Conflict(new { mensaje = "No se ha encontrado el usuario en la BBDD" });
            }
        }

    }
}
