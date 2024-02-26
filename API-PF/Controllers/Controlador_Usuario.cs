using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace API_PF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Usuario : Controller
    {
        private readonly Contexto contexto;
        public Controlador_Usuario(Contexto contexto)
        {
            this.contexto = contexto;
        }
        /// <summary>
        /// Método que devuelve una lista de posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllPosts()
        {
            try
            {
                var posts = contexto.Posts.ToList();//guardo todos los usuarios en la lista de usuarios
                return Ok(posts);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "No se ha podido obtener la lista de posts." });
            }
        }
        /// <summary>
        /// Método que devuelve todos los comentarios de un post en concreto
        /// </summary>
        /// <param name="idPost"></param>
        /// <returns></returns>
        [HttpGet("AllComentarios/{idPost}")]
        public IActionResult AllComentarios(string idPost)
        {
            try
            {
                int idPostInt = int.Parse(idPost);
                var comentarios = contexto.Comentarios.Where(c => c.PostId == idPostInt).ToList();
                return Ok(comentarios);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "No se ha podido obtener la lista de posts." });
            }
        }
        /// <summary>
        /// Método que sirve para cambiar la imagen de un usuario que ya existe
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("CambiarImagen")]
        public IActionResult CambiarImagen([FromBody] Usuario usuario)
        {
            try
            {
                var usuarioExistente = contexto.usuarios.FirstOrDefault(p => p.id_usuario == usuario.id_usuario);
                usuarioExistente.imagen_usuario = usuario.imagen_usuario;
                
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error
                return Conflict(new { mensaje = "Error al registrar el post." });
            }
        }
        /// <summary>
        /// Método para crear un nuevo post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegistrarPost([FromBody] Post post)
        {
            try
            {
                var usuarioExistenteEmail = contexto.usuarios.FirstOrDefault(p => p.id_usuario == post.UsuarioId);
                if (usuarioExistenteEmail == null)
                {
                    // Si el usuario no se encuentra
                    return Conflict(new { mensaje = "El usuario especificado no existe." });
                }
                contexto.Posts.Add(post);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error
                return Conflict(new { mensaje = "Error al registrar el post." });
            }
        }
        /// <summary>
        /// Método para crear un comentario nuevo
        /// </summary>
        /// <param name="comentario"></param>
        /// <returns></returns>
        [HttpPost("CrearComentario")]
        public IActionResult CrearComentario([FromBody] Comentario comentario)
        {
            try
            {
                var usuario = contexto.usuarios.FirstOrDefault(u => u.id_usuario == comentario.UsuarioId);
                if (usuario == null)
                {
                    // Si el usuario no se encuentra
                    return Conflict(new { mensaje = "El usuario especificado no existe." });
                }

                contexto.Comentarios.Add(comentario);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                
                return Conflict(new { mensaje = "Error al registrar el post." });
            }
        }
    }
}
