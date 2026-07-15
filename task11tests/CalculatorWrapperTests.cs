using System;
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
    public void CalculatorGenerator_ValidCode_CompilesSuccessfully()
    {
        var calc = CalculatorGenerator.CreateCalculator();
        Assert.NotNull(calc);
        Assert.Equal(10, calc.Add(7, 3));
    }
}
