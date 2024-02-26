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
                Utils.Utils.Log("Error al obtener la lista de usuarios");
                return Conflict(new { mensaje = "No se ha podido obtener la lista de usuarios." });
            }
        }
        /// <summary>
        /// Método para obtener todos los posts
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllPosts")]
        public IActionResult AllPosts()
        {
            try
            {
                var posts = contexto.Posts.ToList();//guardo todos los posts en una lista
                return Ok(posts);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                Utils.Utils.Log("error al obtener la lista de post");
                return Conflict(new { mensaje = "No se ha podido obtener la lista de posts." });
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
                var usuario = contexto.usuarios.Find(id);//el usuario que tenga esa id
                if (usuario == null)
                {
                    return Conflict(new { mensaje = "No se ha encontrado el usuario" });
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                Utils.Utils.Log("error al obtener un usuario por la id:"+id);
                return Conflict(new { mensaje = "No se ha podido obtener el usuario" });
            }
        }
        /// <summary>
        /// Método para encontrar el usuario por su nombre completo
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenerUsuarioNombre/{nombreUsuario}")]
        public ActionResult<Usuario> ObtenerUsuarioNombre(string nombreUsuario)
        {
            try
            {
                var usuario = contexto.usuarios.FirstOrDefault(u => u.nombreCompleto_usuario == nombreUsuario);
                if (usuario == null)
                {
                    return Conflict(new { mensaje = "No se ha encontrado el usuario" });
                }
                return Ok(usuario);
            }
            catch(Exception ex)
            {
                Utils.Utils.Log("error al obtener un usuario con el nombre:"+nombreUsuario);
                return Conflict(new { mensaje = "No se ha podido obtener el usuario" });
            }            
        }
        /// <summary>
        /// Método para editar un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("EditarUsuario")]
        public IActionResult EditarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.id_usuario == usuario.id_usuario);//busco el usuario por su id
                //si existe cambio los datos por el del usuario que me viene
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
                    Utils.Utils.Log("error al editar un usuario");
                    return Conflict(new { mensaje = "No se ha encontrado el usuario en la BBDD" });
                }
            }
            catch(Exception ex)
            {
                return Conflict(new { mensaje = "Error al editar el usuario" });
            }
            
        }
        /// <summary>
        /// Método para borrar un post
        /// </summary>
        /// <param name="idPost"></param>
        /// <returns></returns>
        [HttpDelete("BorrarPost/{idPost}")]
        public IActionResult BorrarPost(int idPost)
        {
            try
            {
                var post = contexto.Posts.Find(idPost);//busco el post por el id

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
                Utils.Utils.Log("error al borrar un post con id:"+idPost);
                return Conflict(new { mensaje = "Error al borrar el post" });
            }
        }
        /// <summary>
        /// Método para borrar un usuario
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        [HttpDelete("EliminarUsuario/{id_usuario}")]
        public IActionResult EliminarUsuario(int id_usuario)
        {
            try
            {
                var usuario = contexto.usuarios.Find(id_usuario);//lo busco por su id

                if (usuario == null || usuario.rol_usuario == "ADMIN")
                {
                    return Conflict(new { mensaje = "El usuario no fue encontrado o no se puede eliminar" });
                }
                contexto.usuarios.Remove(usuario);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Utils.Utils.Log("Error al eliminar un usuario con id:"+id_usuario);
                return Conflict(new { mensaje = "Error al borrar el usuario" });
            }           
        }
    }
}
