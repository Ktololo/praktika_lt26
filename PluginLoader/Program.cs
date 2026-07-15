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
        if (!Directory.Exists(pluginsDir))
        {
            Console.WriteLine($"Папка Plugins не найдена");
            return;
        }
        var dlls = Directory.GetFiles(pluginsDir, "*.dll");
        if (!dlls.Any())
        {
            Console.WriteLine("Нет DLL в папке Plugins");
            return;
        }
        var assemblies = new List<Assembly>();
        foreach (var dll in dlls)
        {
            try
            {
                assemblies.Add(Assembly.LoadFrom(dll));
            }
            catch (BadImageFormatException ex)
            {
                Console.WriteLine($"Ошибка загрузки {Path.GetFileName(dll)}: неверный формат");
            }
            catch (FileLoadException ex)
            {
                Console.WriteLine($"Ошибка загрузки {Path.GetFileName(dll)}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {Path.GetFileName(dll)}: {ex.Message}");
            }
        }
        var pluginTypes = new List<Type>();
        foreach (var asm in assemblies)
        {
            try
            {
                var types = asm.GetTypes()
                    .Where(t => t.IsClass && t.GetCustomAttribute<PluginLoadAttribute>() != null)
                    .ToList();
                pluginTypes.AddRange(types);
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Ошибка чтения типов: {ex.Message}");
            }
        }

        if (!pluginTypes.Any())
        {
            Console.WriteLine("Плагины не найдены");
            return;
        }
        var sorted = TopologicalSort(pluginTypes);
        foreach (var type in sorted)
        {
            try
            {
                var instance = Activator.CreateInstance(type);
                var command = instance as ICommand;
                command?.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения {type.Name}: {ex.Message}");
            }
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
