using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;

namespace Utilities.Helpers
{
    public interface IAuthHeaderHelper
    {
        string ExtractBearerToken(HttpRequest request);
        (string username, string password) ExtractBasicAuth(HttpRequest request);
        bool TryGetBearerToken(HttpRequest request, out string token);
    }

    public class AuthHeaderHelper : IAuthHeaderHelper
    {
        public string ExtractBearerToken(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string authHeader = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;

            return authHeader.Substring("Bearer ".Length).Trim();
        }

        public (string username, string password) ExtractBasicAuth(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string authHeader = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return (null, null);

            string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
            string decodedCredentials = System.Text.Encoding.UTF8.GetString(decodedBytes);

            int colonIndex = decodedCredentials.IndexOf(':');
            if (colonIndex < 0)
                return (null, null);

            string username = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);

            return (username, password);
        }

        public bool TryGetBearerToken(HttpRequest request, out string token)
        {
            token = ExtractBearerToken(request);
            return !string.IsNullOrEmpty(token);
        }
    }
}