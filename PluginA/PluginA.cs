using System;

[PluginLoad]
public class PluginA : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginA выполнен");
    }
}