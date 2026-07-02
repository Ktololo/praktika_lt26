using System;
using System.IO;
using System.Linq;
public class DirectorySizeCommand : ICommand
{
    private readonly string _directoryPath;
    public DirectorySizeCommand(string directoryPath)
    {
        _directoryPath = directoryPath;
    }
    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Console.WriteLine($"Каталог {_directoryPath} не существует.");
            return;
        }
        var files = Directory.GetFiles(_directoryPath, "*", SearchOption.AllDirectories);
        long totalSize = files.Sum(f => new FileInfo(f).Length);

        Console.WriteLine($"Размер каталога {_directoryPath}: {totalSize} байт");
    }
}