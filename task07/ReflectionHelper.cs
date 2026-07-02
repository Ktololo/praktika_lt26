using System;
using System.Reflection;
using System.Text;
public static class ReflectionHelper
{
    public static string PrintTypeInfo(Type type)
    {
        var result = new StringBuilder();
        var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
        result.AppendLine(displayName != null ? $"Отображаемое имя: {displayName.DisplayName}" : $"Имя класса: {type.Name}");
        var version = type.GetCustomAttribute<VersionAttribute>();
        if (version != null)
            result.AppendLine($"Версия класса: {version.Major}.{version.Minor}");
        result.AppendLine("Методы:");
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var attr = method.GetCustomAttribute<DisplayNameAttribute>();
            result.AppendLine($"  {method.Name} ({attr?.DisplayName ?? "без отображаемого имени"})");
        }
        result.AppendLine("Свойства:");
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
            result.AppendLine($"  {prop.Name} ({attr?.DisplayName ?? "без отображаемого имени"})");
        }
        return result.ToString();
    }
}