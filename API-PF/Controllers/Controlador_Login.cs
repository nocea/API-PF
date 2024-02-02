using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var salt = config.GetSection("Salt");
                string stringSalt = salt["string"];
                usuarioLogin.passwd_usuario= HashPassword(usuarioLogin.passwd_usuario, stringSalt);
                Console.WriteLine(usuarioLogin.passwd_usuario);
                Console.WriteLine(usuarioExistente.passwd_usuario);
                if (usuarioExistente!=null)
                {
                    if (usuarioLogin.passwd_usuario== usuarioExistente.passwd_usuario)
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
