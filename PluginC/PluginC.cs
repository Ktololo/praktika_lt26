using System;

[PluginLoad(DependsOn = "PluginA")]
public class PluginC : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginC выполнен (зависит от PluginA)");
    }
}