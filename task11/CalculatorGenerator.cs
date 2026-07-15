using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public static class CalculatorGenerator
{
    private static readonly string _code = @"
using System;

public class Calculator : ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b)
    {
        if (b == 0) throw new DivideByZeroException(""Деление на ноль"");
        return a / b;
    }
}";
    public static ICalculator CreateCalculator()
    {
        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_code);
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
            };
            var compilation = CSharpCompilation.Create("CalculatorAssembly")
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(references)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
                throw new Exception($"Ошибка компиляции:\n{errors}");
            }
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            var type = assembly.GetType("Calculator");
            return (ICalculator)Activator.CreateInstance(type);
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось создать калькулятор: {ex.Message}", ex);
        }
    }
}
