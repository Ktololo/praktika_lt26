using System;
using System.IO;
using System.Linq;
public class DirectorySizeCommand : ICommand
{
    private readonly string _path;
    public DirectorySizeCommand(string path)
    {
        _path = path;
    }
    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Каталог не найден: {_path}");
            return;
        }
        var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
        long size = files.Sum(f => new FileInfo(f).Length);
        Console.WriteLine($"Размер каталога: {size} байт");
    }
}
