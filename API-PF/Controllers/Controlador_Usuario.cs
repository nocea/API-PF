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
        [HttpPost]
        public IActionResult RegistrarUsuario([FromBody] Post post)
        {
            try
            {
                var usuarioExistenteEmail = contexto.usuarios.FirstOrDefault(p => p.id_usuario == post.UsuarioId);
                if (usuarioExistenteEmail == null)
                {
                    // Si el usuario no se encuentra, retornar un error
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
        [HttpPost("CrearComentario")]
        public IActionResult CrearComentario([FromBody] Comentario comentario)
        {
            try
            {
                var usuario = contexto.usuarios.FirstOrDefault(u => u.id_usuario == comentario.UsuarioId);
                if (usuario == null)
                {
                    // Si el usuario no se encuentra, retornar un error
                    return Conflict(new { mensaje = "El usuario especificado no existe." });
                }

                contexto.Comentarios.Add(comentario);
                contexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error
                return Conflict(new { mensaje = "Error al registrar el post." });
            }
        }
    }
}
