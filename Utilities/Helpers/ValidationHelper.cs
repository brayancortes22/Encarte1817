using System;
using FluentValidation.Results;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Utilities.Helpers
{
    /// <summary>
    /// Implementación de la interfaz IValidationHelper que proporciona funcionalidades
    /// para la validación de diferentes tipos de datos como números telefónicos,
    /// contraseñas, URLs, direcciones IP, tarjetas de crédito y documentos de identidad.
    /// </summary>
    public class ValidationHelper : IValidationHelper
    {
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var normalizedPhone = Regex.Replace(phoneNumber, @"[\s\-\(\)]", string.Empty);

            var regex = new Regex(@"^\+?[0-9]{8,15}$");
            return regex.IsMatch(normalizedPhone);
        }

        public bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

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

            var ipv4Regex = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}" +
                                      @"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

            if (ipv4Regex.IsMatch(ipAddress))
                return true;

            var ipv6Regex = new Regex(@"^([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,7}:$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}$" +
                                      @"|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}$" +
                                      @"|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})$" +
                                      @"|:((:[0-9a-fA-F]{1,4}){1,7}|:)$");

            return ipv6Regex.IsMatch(ipAddress);
        }

        public bool IsValidCreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            var normalizedCard = Regex.Replace(cardNumber, @"[\s\-]", string.Empty);

            if (!normalizedCard.All(char.IsDigit))
                return false;

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

            var normalizedId = Regex.Replace(identityNumber, @"[\s\-]", string.Empty);

            if (normalizedId.Length < 8 || normalizedId.Length > 15)
                return false;

            return normalizedId.All(c => char.IsLetterOrDigit(c));
        }


        public ValidationResult Validate<T>(T dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var validator = ValidatorFactory.GetValidator<T>();
            if (validator == null)
                throw new InvalidOperationException($"No se encontró un validador para el tipo {typeof(T).Name}");

            return validator.Validate(dto);
        }
    }
}
