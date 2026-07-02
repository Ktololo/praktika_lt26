using System;
using System.IO;

public class FindFilesCommand : ICommand
{
    private readonly string _directoryPath;
    private readonly string _mask;
    public FindFilesCommand(string directoryPath, string mask)
    {
        _directoryPath = directoryPath;
        _mask = mask;
    }

    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Console.WriteLine($"Каталог {_directoryPath} не существует.");
            return;
        }
        var files = Directory.GetFiles(_directoryPath, _mask, SearchOption.AllDirectories);
        Console.WriteLine($"Найдено файлов по маске '{_mask}': {files.Length}");
        foreach (var file in files)
        {
            Console.WriteLine($"  {file}");
        }
    }
}