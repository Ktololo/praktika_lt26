using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;

public static class CalculatorGenerator
{
    private static readonly string _code = @"
public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b)
    {
        if (b == 0) throw new System.DivideByZeroException(""Нельзя делить на ноль"");
        return a / b;
    }
}";
    public static object CreateCalculator()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(_code);
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
        };
        var compilation = CSharpCompilation.Create("CalculatorAssembly")
            .AddSyntaxTrees(syntaxTree)
            .AddReferences(references)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using (var ms = new System.IO.MemoryStream())
        {
            var result = compilation.Emit(ms);
            if (!result.Success) throw new Exception("Ошибка компиляции");
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            var type = assembly.GetType("калькулятор");
            return Activator.CreateInstance(type);
        }
    }
}