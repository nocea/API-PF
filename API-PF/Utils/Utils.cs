using System.Security.Cryptography;
using System.Text;

namespace API_PF.Utils
{
    public static class Utils
    {
        public static string HashPassword(string password, string salt)
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
        // Ruta del archivo de registro
        private static readonly string logFilePath = "log.txt";
        // Método para escribir en el registro
        public static void Log(string message)
        {
            // Obtiene la marca de tiempo actual.
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Escribe el mensaje de registro en el archivo.
            using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine($"[{timeStamp}] {message}");
            }

            // Muestra el mensaje en la consola (opcional).
            Console.WriteLine($"[{timeStamp}] {message}");
        }
    }
}
