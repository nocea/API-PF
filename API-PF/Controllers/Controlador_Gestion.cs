using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PF.Controllers
{   /// <summary>
    /// Controlador para la administración de la aplicación
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Gestion : Controller
    {
        private readonly Contexto contexto;//instancio el contexto para poder usarlo

        public Controlador_Gestion(Contexto contexto)
        {
            this.contexto = contexto;
        }
        /// <summary>
        /// Método para devolver todos los usuarios de la BBDD
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            try
            {
                var usuarios = contexto.usuarios.ToList();//guardo todos los usuarios en la lista de usuarios
                return Ok(usuarios);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-ObtenerUsuarios()]No se ha podido obtener la lista de usuarios." });
            }
        }
        [HttpGet("AllPosts")]
        public IActionResult AllPosts()
        {
            try
            {
                var posts = contexto.Posts.ToList();//guardo todos los usuarios en la lista de usuarios
                return Ok(posts);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-ObtenerUsuarios()]No se ha podido obtener la lista de posts." });
            }
        }
        /// <summary>
        /// Método que devuelve un usuario según la id introducida
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Usuario> ObtenerUsuario(int id)
        {
            try
            {
                var usuario = contexto.usuarios.Find(id);
                if (usuario == null)
                {
                    return Conflict(new { mensaje = "[ERROR-ObtenerUsuario(int id)]No se ha encontrado el usuario" });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-ObtenerUsuario(int id)]No se ha podido obtener el usuario" });
            }
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
            catch(Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-ObtenerUsuarioNombre(string nombreUsuario)]No se ha podido obtener el usuario" });
            }            
        }
        
        [HttpPost("EditarUsuario")]
        public IActionResult EditarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.id_usuario == usuario.id_usuario);

                if (usuarioExistente != null)
                {
                    usuarioExistente.alias_usuario = usuario.alias_usuario;
                    usuarioExistente.email_usuario = usuario.email_usuario;
                    usuarioExistente.movil_usuario = usuario.movil_usuario;
                    usuarioExistente.nombreCompleto_usuario = usuario.nombreCompleto_usuario;
                    contexto.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Conflict(new { mensaje = "[ERROR-EditarUsuario([FromBody] Usuario usuario)]No se ha encontrado el usuario en la BBDD" });
                }
            }
            catch(Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-EditarUsuario([FromBody] Usuario usuario)]Error al editar el usuario" });
            }
            
        }
        [HttpDelete("BorrarPost/{idPost}")]
        public IActionResult BorrarPost(int idPost)
        {
            try
            {
                var post = contexto.Posts.Find(idPost);

                if (post == null )
                {
                    return Conflict(new { mensaje = "El post no fue encontrado o no se puede eliminar" });
                }
                contexto.Posts.Remove(post);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-EliminarUsuario(int id_usuario)]Error al borrar el usuario" });
            }
        }
        [HttpDelete("EliminarUsuario/{id_usuario}")]
        public IActionResult EliminarUsuario(int id_usuario)
        {
            try
            {
                var usuario = contexto.usuarios.Find(id_usuario);

                if (usuario == null || usuario.rol_usuario == "ADMIN")
                {
                    return Conflict(new { mensaje = "[ERROR-EliminarUsuario(int id_usuario)]El usuario no fue encontrado o no se puede eliminar" });
                }
                contexto.usuarios.Remove(usuario);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "[ERROR-EliminarUsuario(int id_usuario)]Error al borrar el usuario" });
            }           
        }
    }
}
