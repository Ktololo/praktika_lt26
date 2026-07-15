using System;
using System.IO;
using System.Reflection;
using System.Linq;
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
}
