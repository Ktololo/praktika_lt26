using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        string pluginsDir = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        Directory.CreateDirectory(pluginsDir);
        var dlls = Directory.GetFiles(pluginsDir, "*.dll");
        var assemblies = dlls.Select(Assembly.LoadFrom).ToList();
        var pluginTypes = new List<Type>();
        foreach (var asm in assemblies)
        {
            var types = asm.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttribute<PluginLoadAttribute>() != null)
                .ToList();
            pluginTypes.AddRange(types);
        }
        var sorted = TopologicalSort(pluginTypes);
        foreach (var type in sorted)
        {
            var instance = Activator.CreateInstance(type);
            var command = instance as ICommand;
            command?.Execute();
        }
    }
    static List<Type> TopologicalSort(List<Type> types)
    {
        var result = new List<Type>();
        var visited = new HashSet<Type>();

        foreach (var type in types)
            Visit(type, types, visited, result);

        return result;
    }
    static void Visit(Type type, List<Type> all, HashSet<Type> visited, List<Type> result)
    {
        if (visited.Contains(type)) return;
        visited.Add(type);

        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        if (!string.IsNullOrEmpty(attr?.DependsOn))
        {
            var dep = all.FirstOrDefault(t => t.Name == attr.DependsOn);
            if (dep != null)
                Visit(dep, all, visited, result);
        }
        result.Add(type);
    }
}