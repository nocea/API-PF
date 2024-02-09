using BIBLIOTECA;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace API_PF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controlador_Recuperar : ControllerBase
    {
        private readonly Contexto contexto;

        public Controlador_Recuperar(Contexto contexto)
        {
            this.contexto = contexto;
        }

        [HttpPost("RecuperarContrasena")]
        public IActionResult RecuperarContrasena([FromBody] Usuario usuarioRecuperar)
        {
            try
            {
                
                var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.email_usuario == usuarioRecuperar.email_usuario);
                if (usuarioExistente != null)
                {
                    var nuevoToken = new Token
                    {
                        cadena_token = GenerarNuevoToken(),
                        fechaFin_token = DateTime.Now.AddHours(24)

                    };
                    contexto.tokens.Add(nuevoToken);
                    contexto.SaveChanges();
                    EnviarCorreoRecuperacion(usuarioExistente.email_usuario, nuevoToken.cadena_token);
                    return Ok(new { mensaje = "Token-Hecho" });
                }
                else
                {
                    return Conflict(new { mensaje = "Email no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error al recuperar contraseña.", Error = ex.Message });
            }
        }
        [HttpPost("CambiarContrasena/{token}")]
        public IActionResult CambiarContrasena([FromBody] Usuario usuarioEmailToken,string token)
        {
            try
            {
                var usuarioExistente = contexto.usuarios.FirstOrDefault(u => u.email_usuario == usuarioEmailToken.email_usuario);
                var tokenValido = contexto.tokens.FirstOrDefault(t => t.cadena_token == token);
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var salt = config.GetSection("Salt");
                string stringSalt = salt["string"];
                if (usuarioExistente != null && tokenValido != null)
                {
                    if (tokenValido.fechaFin_token < DateTime.Now)
                    {
                        return Conflict(new { mensaje = "Tiempo de uso token pasado" });
                    }
                    else
                    {
                        string contraseñaEncriptada = HashPassword(usuarioEmailToken.passwd_usuario, stringSalt);
                        usuarioExistente.passwd_usuario = contraseñaEncriptada;
                        contexto.SaveChanges();
                        return Ok(new { mensaje = "token ok-email ok-contraseña nueva ok" });
                    }
                }
                else
                {
                    return Conflict(new { mensaje = "Email o token no valido" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error al cambiar contraseña", Error = ex.Message });
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
        private string GenerarNuevoToken()
        {
            Guid guid = Guid.NewGuid();
            string token = guid.ToString();
            return token;
        }
        private void EnviarCorreoRecuperacion(string destinatario, string token)
        {
            try
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var correoConfiguracion = config.GetSection("CorreoConfiguracion");
                string urlRecuperacion = "https://localhost:7237/Controlador_Recuperar/CambiarContrasena?email="+destinatario+"&token="+token;

                
                var smtpClient = new SmtpClient(correoConfiguracion["ServidorSmtp"])
                {
                    Port = int.Parse(correoConfiguracion["Puerto"]),
                    Credentials = new NetworkCredential(correoConfiguracion["Usuario"], correoConfiguracion["Contrasena"]),
                    EnableSsl = true,
                };

                // Crear el mensaje de correo
                var mensaje = new MailMessage
                {
                    From = new MailAddress(correoConfiguracion["Usuario"]),
                    Subject = "Recuperación de Contraseña",
                    Body = $"Haz clic en el siguiente enlace para recuperar tu contraseña: <a href='{urlRecuperacion}'>{urlRecuperacion}</a>",
                    IsBodyHtml = true,
                };

                // Añadir destinatario
                mensaje.To.Add(destinatario);

                // Enviar el correo
                smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {
                // Manejar errores relacionados con el envío de correo (puedes registrarlos, lanzar una excepción personalizada, etc.)
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}
