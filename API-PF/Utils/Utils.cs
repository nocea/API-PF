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
    }
}
