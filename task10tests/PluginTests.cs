using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

public class PluginTests
{
    [Fact]
    public void PluginA_ShouldExecuteWithoutError()
    {
        var type = typeof(PluginA);
        var instance = Activator.CreateInstance(type);
        var command = instance as ICommand;
        
        Assert.NotNull(command);
        var ex = Record.Exception(() => command.Execute());
        Assert.Null(ex);
    }
    [Fact]
    public void PluginB_ShouldExecuteWithoutError()
    {
        var type = typeof(PluginB);
        var instance = Activator.CreateInstance(type);
        var command = instance as ICommand;
        Assert.NotNull(command);
        var ex = Record.Exception(() => command.Execute());
        Assert.Null(ex);
    }
    [Fact]
    public void PluginC_ShouldExecuteWithoutError()
    {
        var type = typeof(PluginC);
        var instance = Activator.CreateInstance(type);
        var command = instance as ICommand;
        Assert.NotNull(command);
        var ex = Record.Exception(() => command.Execute());
        Assert.Null(ex);
    }

    [Fact]
    public void PluginC_DependsOnPluginA()
    {
        var attr = typeof(PluginC).GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("PluginA", attr.DependsOn);
    }

    [Fact]
    public void PluginLoader_ShouldLoadPluginsWithoutError()
    {
        var pluginsDir = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        Directory.CreateDirectory(pluginsDir);
        var dlls = Directory.GetFiles(pluginsDir, "*.dll");
        var assemblies = dlls.Select(Assembly.LoadFrom).ToList();
        var pluginTypes = new List<Type>();
        foreach (var asm in assemblies)
        {
            var types = asm.GetTypes()
                .Where(t => t.IsClass && t.GetCustomAttribute<PluginLoadAttribute>() != null)
                .ToList();
            pluginTypes.AddRange(types);
        }

        Assert.NotEmpty(pluginTypes);
    }

    [Fact]
    public void PluginLoader_TopologicalSort_ShouldOrderCorrectly()
    {
        var types = new List<Type> { typeof(PluginC), typeof(PluginA), typeof(PluginB) };
        var sorted = TopologicalSort(types);
        Assert.Equal(3, sorted.Count);
        Assert.True(sorted.IndexOf(typeof(PluginA)) < sorted.IndexOf(typeof(PluginC)));
    }

    private List<Type> TopologicalSort(List<Type> types)
    {
        var result = new List<Type>();
        var visited = new HashSet<Type>();
        foreach (var type in types)
            Visit(type, types, visited, result);
        return result;
    }

    private void Visit(Type type, List<Type> all, HashSet<Type> visited, List<Type> result)
    {
        if (visited.Contains(type)) return;
        visited.Add(type);
        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        if (!string.IsNullOrEmpty(attr?.DependsOn))
        {
            var dep = all.FirstOrDefault(t => t.Name == attr.DependsOn);
            if (dep != null)
                Visit(dep, all, visited, result);
        }
        result.Add(type);
    }
}
