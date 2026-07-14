using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Анализатор метаданных\n");
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите путь к DLL");
            return;
        }
        string dllPath = args[0];

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Файл не найден: {dllPath}");
            return;
        }
        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Ошибка загрузки типов: {ex.Message}");
                types = ex.Types.Where(t => t != null).ToArray();
                Console.WriteLine($"Загружено {types.Length} типов из {ex.Types.Length}");
            }
            Console.WriteLine($"Библиотека: {assembly.GetName().Name}");
            Console.WriteLine($"Всего классов: {types.Length}\n");
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsAbstract || type.IsInterface)
                    continue;
                Console.WriteLine($"Класс: {type.FullName}");
                var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (ctors.Any())
                {
                    Console.WriteLine("Конструкторы:");
                    foreach (var c in ctors)
                    {
                        var p = string.Join(", ", c.GetParameters().Select(x => $"{x.ParameterType.Name} {x.Name}"));
                        Console.WriteLine($"  {type.Name}({p})");
                    }
                }
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (methods.Any())
                {
                    Console.WriteLine("Методы:");
                    foreach (var m in methods)
                    {
                        var p = string.Join(", ", m.GetParameters().Select(x => $"{x.ParameterType.Name} {x.Name}"));
                        Console.WriteLine($"  {m.ReturnType.Name} {m.Name}({p})");
                    }
                }
                Console.WriteLine();
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Файл не найден: {ex.FileName}");
        }
        catch (BadImageFormatException ex)
        {
            Console.WriteLine($"Некорректная сборка: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
        }
    }
}
