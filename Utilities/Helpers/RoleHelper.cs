using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Model;
using Entity.Dtos;

namespace Utilities.Helpers
{
    public interface IRoleHelper
    {
        bool HasPermission(IEnumerable<string> userRole, string requiredPermission);
        bool IsInRole(IEnumerable<string> userRole, string roleName);
        bool HasAnyRole(IEnumerable<string> userRole, IEnumerable<string> requiredRole);
        string GetHighestRole(IEnumerable<string> userRole, IDictionary<string, int> rolePriorities);
    }

    public class RoleHelper : IRoleHelper
    {
        public bool HasPermission(IEnumerable<string> userRole, string requiredPermission)
        {
            if (userRole == null || string.IsNullOrWhiteSpace(requiredPermission))
                return false;

            // Aquí podrías implementar una lógica más compleja de permisos
            // Este es un ejemplo simple donde asumimos que el admin tiene todos los permisos
            if (userRole.Contains("Admin", StringComparer.OrdinalIgnoreCase))
                return true;

            // Verificar si el usuario tiene el permiso específico
            return userRole.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase);
        }

        public bool IsInRole(IEnumerable<string> userRole, string roleName)
        {
            if (userRole == null || string.IsNullOrWhiteSpace(roleName))
                return false;

            return userRole.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasAnyRole(IEnumerable<string> userRole, IEnumerable<string> requiredRole)
        {
            if (userRole == null || requiredRole == null || !requiredRole.Any())
                return false;

            return userRole.Intersect(requiredRole, StringComparer.OrdinalIgnoreCase).Any();
        }

        public string GetHighestRole(IEnumerable<string> userRole, IDictionary<string, int> rolePriorities)
        {
            if (userRole == null || !userRole.Any() || rolePriorities == null || !rolePriorities.Any())
                return null;

            int highestPriority = -1;
            string highestRole = null;

            foreach (var role in userRole)
            {
                if (rolePriorities.TryGetValue(role, out int priority) && priority > highestPriority)
                {
                    highestPriority = priority;
                    highestRole = role;
                }
            }

            return highestRole;
        }
    }
}