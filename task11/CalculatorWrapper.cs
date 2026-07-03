using System;
using System.Reflection;

public class CalculatorWrapper
{
    private readonly object _calc;
    private readonly MethodInfo _add;
    private readonly MethodInfo _minus;
    private readonly MethodInfo _mul;
    private readonly MethodInfo _div;
    public CalculatorWrapper()
    {
        _calc = CalculatorGenerator.CreateCalculator();
        var t = _calc.GetType();
        _add = t.GetMethod("Add");
        _minus = t.GetMethod("Minus");
        _mul = t.GetMethod("Mul");
        _div = t.GetMethod("Div");
    }
    public int Add(int a, int b) => (int)_add.Invoke(_calc, new object[] { a, b });
    public int Minus(int a, int b) => (int)_minus.Invoke(_calc, new object[] { a, b });
    public int Mul(int a, int b) => (int)_mul.Invoke(_calc, new object[] { a, b });
    public int Div(int a, int b) => (int)_div.Invoke(_calc, new object[] { a, b });
}