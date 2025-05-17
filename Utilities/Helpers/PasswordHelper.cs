using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
namespace Utilities.Helpers
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
        string GenerateRandomPassword(int length = 12);
    }

    public class PasswordHelper : IPasswordHelper
    {
        // El parámetro WorkFactor determina la complejidad del hash
        // Valores típicos están entre 10-12, aumentar este valor hace el hashing más lento
        // pero más seguro contra ataques de fuerza bruta
        private readonly int _workFactor = 12;

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
                
            // BCrypt automáticamente genera un salt aleatorio y lo incluye en el hash resultante
            return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                return false;
                
            if (string.IsNullOrEmpty(providedPassword))
                return false;
                
            // BCrypt.Verify compara el password dado con el hash almacenado
            // incluyendo la extracción del salt del hash almacenado
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }

        public string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=";
            var random = new Random();
            var chars = new char[length];

            // Asegurar que la contraseña incluya al menos un carácter de cada tipo
            chars[0] = validChars[random.Next(0, 26)]; // minúscula
            chars[1] = validChars[random.Next(26, 52)]; // mayúscula
            chars[2] = validChars[random.Next(52, 62)]; // número
            chars[3] = validChars[random.Next(62, validChars.Length)]; // símbolo

            // Llenar el resto de la contraseña con caracteres aleatorios
            for (int i = 4; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            // Mezclar la contraseña para que los caracteres específicos no estén siempre al principio
            for (int i = 0; i < length; i++)
            {
                int swapIndex = random.Next(length);
                (chars[i], chars[swapIndex]) = (chars[swapIndex], chars[i]); // Utiliza C# 7+ tuple swap
            }

            return new string(chars);
        }
    }
}