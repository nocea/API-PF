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
        [HttpGet("AllComentarios")]
        public IActionResult AllComentarios()
        {
            try
            {
                var comentarios = contexto.Comentarios.ToList();//guardo todos los usuarios en la lista de usuarios
                return Ok(comentarios);//devuelvo el mensaje de ok y la lista
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "No se ha podido obtener la lista de posts." });
            }
        }
        [HttpPost]
        public IActionResult RegistrarUsuario([FromBody] Post post)
        {
            try
            {
                var usuarioExistenteEmail = contexto.usuarios.FirstOrDefault(p => p.id_usuario == post.UsuarioId);
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
