public class CalculatorWrapper
{
    private readonly ICalculator _calc;

    public CalculatorWrapper()
    {
        _calc = CalculatorGenerator.CreateCalculator();
    }
    public int Add(int a, int b) => _calc.Add(a, b);
    public int Minus(int a, int b) => _calc.Minus(a, b);
    public int Mul(int a, int b) => _calc.Mul(a, b);
    public int Div(int a, int b) => _calc.Div(a, b);
}
