using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

public class CalculatorWrapperTests
{
    [Fact]
    public void Add_Works()
    {
        var c = new CalculatorWrapper();
        Assert.Equal(5, c.Add(2, 3));
        Assert.Equal(0, c.Add(-2, 2));
        Assert.Equal(100, c.Add(50, 50));
    }

    [Fact]
    public void Minus_Works()
    {
        var c = new CalculatorWrapper();
        Assert.Equal(2, c.Minus(5, 3));
        Assert.Equal(-5, c.Minus(0, 5));
        Assert.Equal(10, c.Minus(15, 5));
    }

    [Fact]
    public void Mul_Works()
    {
        var c = new CalculatorWrapper();
        Assert.Equal(6, c.Mul(2, 3));
        Assert.Equal(0, c.Mul(5, 0));
        Assert.Equal(-6, c.Mul(-2, 3));
    }

    [Fact]
    public void Div_Works()
    {
        var c = new CalculatorWrapper();
        Assert.Equal(2, c.Div(6, 3));
        Assert.Equal(3, c.Div(15, 5));
        Assert.Equal(-2, c.Div(-6, 3));
    }

    [Fact]
    public void Div_ByZero_Throws()
    {
        var c = new CalculatorWrapper();
        Assert.Throws<DivideByZeroException>(() => c.Div(5, 0));
    }

    [Fact]
    public void CalculatorGenerator_InvalidCode_ThrowsException()
    {
        var oldCode = typeof(CalculatorGenerator)
            .GetField("_code", BindingFlags.NonPublic | BindingFlags.Static)
            .GetValue(null);

        try
        {
            var field = typeof(CalculatorGenerator)
                .GetField("_code", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, "public class Calculator { }");

            var ex = Assert.Throws<Exception>(() => CalculatorGenerator.CreateCalculator());
            Assert.Contains("Ошибка компиляции", ex.Message);
        }
        finally
        {
            var field = typeof(CalculatorGenerator)
                .GetField("_code", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, oldCode);
        }
    }
}
