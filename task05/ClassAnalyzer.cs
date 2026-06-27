using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ClassAnalyzer
{
    private Type _type;
    public ClassAnalyzer(Type type)
    {
        _type = type;
    }
    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(m => m.Name);
    }
    public IEnumerable<string> GetMethodParams(string methodName)
    {
        var method = _type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        if (method == null)
            return new List<string>();

        return method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}");
    }
    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(f => f.Name);
    }
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(p => p.Name);
    }
    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.GetCustomAttribute<T>() != null;
    }
}