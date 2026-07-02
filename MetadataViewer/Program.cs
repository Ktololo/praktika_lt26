using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Анализатор метаданных библиотеки\n");
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите путь к DLL");
            return;
        }
        string dllPath = args[0];

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Файл {dllPath} не найден");
            return;
        }
        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface)
                .ToList();
            Console.WriteLine($"Библиотека: {assembly.GetName().Name}");
            Console.WriteLine($"Классов: {types.Count}\n");
            foreach (var type in types)
            {
                Console.WriteLine($"Класс: {type.FullName}");
                var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (constructors.Any())
                {
                    Console.WriteLine("Конструкторы:");
                    foreach (var ctor in constructors)
                    {
                        var paramNames = string.Join(", ", ctor.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                        Console.WriteLine($"  {type.Name}({paramNames})");
                    }
                }
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (methods.Any())
                {
                    Console.WriteLine("Методы:");
                    foreach (var method in methods)
                    {
                        var paramNames = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                        Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({paramNames})");
                    }
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}