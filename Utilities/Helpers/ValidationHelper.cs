using System;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Interfaces;

namespace Utilities.Helpers
{
    /// <summary>
    /// Implementación de la interfaz IValidationHelper que proporciona funcionalidades
    /// para la validación de diferentes tipos de datos como números telefónicos,
    /// contraseñas, URLs, direcciones IP, tarjetas de crédito y documentos de identidad.
    /// </summary>
    public class VaidationHelper : IValidationHelper
    {
        /// <summary>
        /// Verifica si un número de teléfono tiene un formato válido.
        /// </summary>
        /// <param name="phoneNumber">El número de teléfono a validar.</param>
        /// <returns>
        /// True si el número de teléfono tiene un formato válido;
        /// False si es nulo, vacío o no cumple con el formato esperado.
        /// </returns>
        /// <remarks>
        /// Se consideran válidos los números que contengan entre 8 y 15 dígitos,
        /// pudiendo incluir un signo "+" al inicio para formato internacional.
        /// Los espacios, guiones y paréntesis son ignorados durante la validación.
        /// </remarks>
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

        /// <summary>
        /// Verifica si una contraseña cumple con los criterios de seguridad establecidos.
        /// </summary>
        /// <param name="password">La contraseña a validar.</param>
        /// <returns>
        /// True si la contraseña es considerada fuerte;
        /// False si es nula, vacía o no cumple con los requisitos de seguridad.
        /// </returns>
        /// <remarks>
        /// Una contraseña fuerte debe tener al menos 8 caracteres y contener:
        /// - Al menos una letra mayúscula
        /// - Al menos una letra minúscula
        /// - Al menos un dígito
        /// - Al menos un carácter especial (no alfanumérico)
        /// </remarks>
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

        /// <summary>
        /// Verifica si una URL tiene un formato válido y utiliza el protocolo HTTP o HTTPS.
        /// </summary>
        /// <param name="url">La URL a validar.</param>
        /// <returns>
        /// True si la URL tiene un formato válido y usa un protocolo soportado;
        /// False si es nula, vacía o no cumple con los requisitos.
        /// </returns>
        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Verifica si una cadena representa una dirección IP válida (IPv4 o IPv6).
        /// </summary>
        /// <param name="ipAddress">La dirección IP a validar.</param>
        /// <returns>
        /// True si la cadena representa una dirección IP válida;
        /// False si es nula, vacía o no tiene un formato correcto de IPv4 o IPv6.
        /// </returns>
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

        /// <summary>
        /// Verifica si un número de tarjeta de crédito es válido utilizando el algoritmo de Luhn.
        /// </summary>
        /// <param name="cardNumber">El número de tarjeta de crédito a validar.</param>
        /// <returns>
        /// True si el número de tarjeta de crédito es válido según el algoritmo de Luhn;
        /// False si es nulo, vacío o no pasa la validación.
        /// </returns>
        /// <remarks>
        /// El método elimina espacios y guiones del número antes de validarlo,
        /// verifica que solo contenga dígitos y aplica el algoritmo de Luhn para
        /// determinar si es un número de tarjeta potencialmente válido.
        /// </remarks>
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

        /// <summary>
        /// Verifica si un número de identificación personal tiene un formato válido.
        /// </summary>
        /// <param name="identityNumber">El número de identificación a validar.</param>
        /// <returns>
        /// True si el número de identificación tiene un formato potencialmente válido;
        /// False si es nulo, vacío o no cumple con los criterios básicos de validación.
        /// </returns>
        /// <remarks>
        /// Esta es una implementación genérica que debe adaptarse a los formatos específicos
        /// de identificación de cada país o región. Actualmente verifica:
        /// - La longitud debe estar entre 8 y 15 caracteres
        /// - Solo debe contener letras y números
        /// Los espacios y guiones son eliminados durante la normalización.
        /// </remarks>
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