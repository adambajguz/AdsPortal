namespace AdsPortal.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class AssemblyExtensions
    {
        public static Assembly[] GetAssemblies(params string[] assemblyFilters)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string assemblyFilter in assemblyFilters)
                assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                                                           .Where(assembly => IsWildcardMatch(assembly?.GetName()?.Name ?? string.Empty, assemblyFilter))
                                                           .ToArray());

            return assemblies.ToArray();
        }

        public static Type[] GetAllExportedTypes(params string[] assemblyFilters)
        {
            IEnumerable<Type> assemblies = GetAssemblies(assemblyFilters).Where(assembly => !assembly.IsDynamic)
                                                                         .SelectMany(GetExportedTypes);

            return assemblies.ToArray();
        }

        public static IEnumerable<Type> GetExportedTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (NotSupportedException)
            {
                // A type load exception would typically happen on an Anonymously Hosted DynamicMethods
                // Assembly and it would be safe to skip this exception.
                return Type.EmptyTypes;
            }
            catch (FileLoadException)
            {
                // The assembly points to a not found assembly - ignore and continue
                return Type.EmptyTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Return the types that could be loaded. Types can contain null values.
                return ex.Types.Where(type => type is not null)!;
            }
            catch (Exception ex)
            {
                // Throw a more descriptive message containing the name of the assembly.
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unable to load types from assembly {0}. {1}", assembly.FullName, ex.Message), ex);
            }
        }

        private static bool IsWildcardMatch(string input, string wildcard)
        {
            if (input == string.Empty)
                return false;

            return input == wildcard || Regex.IsMatch(input, "^" + Regex.Escape(wildcard).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase);
        }
    }
}
