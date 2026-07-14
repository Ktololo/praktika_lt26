using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Загрузка команд из DLL\n");
        string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "FileSystemCommands.dll");

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Файл не найден: {dllPath}");
            Console.WriteLine("Соберите проект FileSystemCommands и поместите DLL в папку с программой.");
            return;
        }
        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
            var commandTypes = assembly.GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();
            if (!commandTypes.Any())
            {
                Console.WriteLine("Команды не найдены.");
                return;
            }

            foreach (var type in commandTypes)
            {
                Console.WriteLine($"Команда: {type.Name}");
                object command = null;

                try
                {
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
                        Console.Write("Введите маску: ");
                        string mask = Console.ReadLine();
                        command = Activator.CreateInstance(type, path, mask);
                    }
                    if (command != null)
                    {
                        ((ICommand)command).Execute();
                    }
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine($"Ошибка создания команды: {ex.InnerException?.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка выполнения команды: {ex.Message}");
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
            Console.WriteLine($"Ошибка загрузки DLL: {ex.Message}");
        }
    }
}
