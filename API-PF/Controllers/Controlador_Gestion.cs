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
            // Obtener todos los usuarios del contexto
            var usuarios = contexto.usuarios.ToList();

            // Devolver los usuarios como resultado
            return Ok(usuarios);
        }
    }
}
