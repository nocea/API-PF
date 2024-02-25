using BIBLIOTECA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using API_PF.Utils;
using System.Net.Mail;
using System.Net;

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
                    return Conflict(new { mensaje = "El correo electrónico ya existe." });
                }

                // Comprueba si el alias ya existe
                var usuarioExistenteAlias = contexto.usuarios.FirstOrDefault(u => u.alias_usuario == nuevoUsuario.alias_usuario);

                if (usuarioExistenteAlias != null)
                {
                    // Usuario con el mismo alias ya registrado
                    return Conflict(new { mensaje = "El alias de usuario ya existe." });
                }
                string contraseñaEncriptada = Utils.Utils.HashPassword(nuevoUsuario.passwd_usuario, stringSalt);
                nuevoUsuario.passwd_usuario = contraseñaEncriptada;
                string rutaImagen = "Utils/fotoInicial.png";
                byte[] imageBytes = ImageToByteArray(rutaImagen);
                nuevoUsuario.imagen_usuario = imageBytes;
                nuevoUsuario.registrado = false;
                // Agrega el nuevo usuario al contexto
                contexto.usuarios.Add(nuevoUsuario);
                // Guarda los cambios en la base de datos
                contexto.SaveChanges();
                EnviarCorreoRecuperacion(nuevoUsuario.email_usuario);
                // Devuelve un código de estado para confirmar que se ha creado el usuario
                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error
                return Conflict(new { mensaje = "Error al registrar el usuario." });
            }
        }
        static byte[] ImageToByteArray(string imagePath)
        {
            // Convertir la imagen en un arreglo de bytes
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return memoryStream.ToArray();
                }
            }
        }
        [HttpPost("ConfirmacionRegistro/{email}")]
        public IActionResult ConfirmacionRegistro(string email)
        {
            try
            {
                var usuarioExistenteEmail = contexto.usuarios.FirstOrDefault(u => u.email_usuario == email);
                if (usuarioExistenteEmail != null)
                {
                    usuarioExistenteEmail.registrado = true;
                    // Devuelve un conflicto con el mensaje
                    contexto.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Conflict(new { mensaje = "Error al encontrar el usuario." });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new { mensaje = "Error al registrar el usuario."});
            }
        }
        private void EnviarCorreoRecuperacion(string destinatario)
        {
            try
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var correoConfiguracion = config.GetSection("CorreoConfiguracion");
                string urlRecuperacion = "https://localhost:7237/Controlador_Registro/ConfirmacionRegistro?email=" + destinatario;


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
                    Subject = "Confirmación registro BlogShip",
                    Body = $"Haz clic en el siguiente enlace para confirmar tu registro: <a href='{urlRecuperacion}'>{urlRecuperacion}</a>",
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
