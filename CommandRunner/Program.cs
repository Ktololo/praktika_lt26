using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Динамическая загрузка команд ===\n");
        string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "FileSystemCommands.dll");
        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Библиотека {dllPath} не найдена.");
            Console.WriteLine("Сначала соберите проект FileSystemCommands.");
            return;
        }
        var assembly = Assembly.LoadFrom(dllPath);
        var commandTypes = assembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();
        if (!commandTypes.Any())
        {
            Console.WriteLine("Не найдено ни одной команды в библиотеке.");
            return;
        }
        foreach (var type in commandTypes)
        {
            Console.WriteLine($"Команда: {type.Name}");
            object command = null;
            if (type.Name == "DirectorySizeCommand")
            {
                Console.Write("Введите путь к каталогу: ");
                string path = Console.ReadLine();
                command = Activator.CreateInstance(type, path);
            }
            else if (type.Name == "FindFilesCommand")
            {
                Console.Write("Введите путь к каталогу: ");
                string path = Console.ReadLine();
                Console.Write("Введите маску файлов: ");
                string mask = Console.ReadLine();
                command = Activator.CreateInstance(type, path, mask);
            }
            if (command != null)
            {
                ((ICommand)command).Execute();
            }
            Console.WriteLine();
        }
    }
}