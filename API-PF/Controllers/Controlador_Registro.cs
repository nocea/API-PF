using BIBLIOTECA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace API_PF.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Registro : ControllerBase
    {
        //instancia del contexto para poder utilizarlo
        private readonly Contexto contexto;
        public Controlador_Registro(Contexto contexto)
        {
            this.contexto = contexto;
        }
        /// <summary>
        /// Método que sirve para traer todos los usuarios de la base de datos a una lista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Usuario> GetUsuarios()
        {
            return contexto.usuarios.ToList();
        }
        /// <summary>
        /// Método que comprueba que el usuario introducido existe,según su email/alias
        /// y si no existe lo crea.
        /// </summary>
        /// <param name="nuevoUsuario"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegistrarUsuario([FromBody] Usuario nuevoUsuario)
        {
            try
            {
                // Comprueba si el email ya existe
                var usuarioExistenteEmail = contexto.usuarios.FirstOrDefault(u => u.email_usuario == nuevoUsuario.email_usuario);
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var salt = config.GetSection("Salt");
                string stringSalt = salt["string"];
                if (usuarioExistenteEmail != null)
                {
                    // Devuelve un conflicto con el mensaje
                    return Conflict(new { Mensaje = "El correo electrónico ya existe." });
                }

                // Comprueba si el alias ya existe
                var usuarioExistenteAlias = contexto.usuarios.FirstOrDefault(u => u.alias_usuario == nuevoUsuario.alias_usuario);

                if (usuarioExistenteAlias != null)
                {
                    // Usuario con el mismo alias ya registrado
                    return Conflict(new { Mensaje = "El alias de usuario ya existe." });
                }
                string contraseñaEncriptada = HashPassword(nuevoUsuario.passwd_usuario, stringSalt);
                nuevoUsuario.passwd_usuario = contraseñaEncriptada;
                Console.WriteLine(nuevoUsuario.movil_usuario);
                // Agrega el nuevo usuario al contexto
                contexto.usuarios.Add(nuevoUsuario);

                // Guarda los cambios en la base de datos
                contexto.SaveChanges();

                // Devuelve un código de estado para confirmar que se ha creado el usuario
                return CreatedAtAction(nameof(GetUsuarios), new { id = nuevoUsuario.id_usuario }, nuevoUsuario);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error
                return StatusCode(500, new { Mensaje = "Error al registrar el usuario.", Error = ex.Message });
            }
            static string HashPassword(string password, string salt)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Concatenar la contraseña y la sal antes de hashear
                    string saltedPassword = password + salt;

                    // Convertir la cadena a bytes y calcular el hash
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                    // Convertir los bytes a una cadena hexadecimal
                    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
            }
        }
        

    }
}
