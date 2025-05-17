using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utilities.Helpers
{
    public interface IValidationHelper
    {
        bool IsValidPhoneNumber(string phoneNumber);
        bool IsStrongPassword(string password);
        bool IsValidUrl(string url);
        bool IsValidIp(string ipAddress);
        bool IsValidCreditCard(string cardNumber);
        bool IsValidIdentityNumber(string identityNumber);
    }

    public class VaidationHelper : IValidationHelper
    {
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Eliminar espacios, guiones y paréntesis para normalizar
            var normalizedPhone = Regex.Replace(phoneNumber, @"[\s\-\(\)]", string.Empty);
            
            // Verificar si tiene un formato internacional válido (ejemplos simples)
            var regex = new Regex(@"^\+?[0-9]{8,15}$");
            return regex.IsMatch(normalizedPhone);
        }

        public bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            // Verificar requisitos comunes de contraseña fuerte
            bool hasUppercase = password.Any(char.IsUpper);
            bool hasLowercase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUppercase && hasLowercase && hasDigit && hasSpecialChar;
        }

        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public bool IsValidIp(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            // Validar IPv4
            var ipv4Regex = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            if (ipv4Regex.IsMatch(ipAddress))
                return true;

            // Validar IPv6 (simplificado)
            var ipv6Regex = new Regex(@"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$");
            return ipv6Regex.IsMatch(ipAddress);
        }

        public bool IsValidCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            // Eliminar espacios y guiones
            var normalizedCard = Regex.Replace(cardNumber, @"[\s\-]", string.Empty);
            
            // Verificar que solo contenga dígitos
            if (!normalizedCard.All(char.IsDigit))
                return false;
                
            // Implementar algoritmo de Luhn (validación básica de tarjetas)
            int sum = 0;
            bool alternate = false;
            for (int i = normalizedCard.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(normalizedCard[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n = (n % 10) + 1;
                }
                sum += n;
                alternate = !alternate;
            }
            
            return (sum % 10 == 0);
        }

        public bool IsValidIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrWhiteSpace(identityNumber))
                return false;
                
            // Esta es una implementación simple. En un caso real, deberías adaptar
            // esto al formato de identificación específico de tu país/región
            var normalizedId = Regex.Replace(identityNumber, @"[\s\-]", string.Empty);
            
            // Verificar longitud adecuada (8-15 caracteres, ajusta según tu caso)
            if (normalizedId.Length < 8 || normalizedId.Length > 15)
                return false;
                
            // Aquí puedes implementar algoritmos de validación específicos
            // por ejemplo, para DNI, NIF, SSN, etc.
            
            // Esta es una validación simple de ejemplo
            return normalizedId.All(c => char.IsLetterOrDigit(c));
        }
    }
}