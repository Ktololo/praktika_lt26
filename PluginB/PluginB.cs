using System;

[PluginLoad]
public class PluginB : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginB выполнен");
    }
}