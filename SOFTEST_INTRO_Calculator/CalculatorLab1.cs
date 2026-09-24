namespace SOFTEST_INTRO_Calculator;

public class CalculatorLab1
{
    public double Add(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;

    public double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new ArgumentException("Throws ArgumentException");
        }

        return a / b;
    }

    public long Factorial(int n)
    {
        if (n == 0)
        {
            return 1;
        }
        else if (n < 0 || n > 20)
        {
            throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException");
        }

        long result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }

    public double TriangleArea(double height, double width)
    {
        if (height < 0 || width < 0)
        {
            throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException");
        }
        else if (height == 0 || width == 0)
        {
            return 0;
        }

        return 0.5 * height * width;
    }

    public double CircleArea(double radius)
    {
        if (radius < 0)
        {
            throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException");
        }
        else if (radius == 0)
        {
            return 0;
        }

        return Math.PI * radius * radius;
    }

    // Function A is the number of permutations: nPr = n! / (n - r)!.
    public long UnknownFunctionA(int n, int r)
    {
        ValidateUnknownFunctionInputs(n, r);
        return Factorial(n) / Factorial(n - r);
    }

    // Function B is the number of combinations: nCr = n! / (r! * (n - r)!).
    public long UnknownFunctionB(int n, int r)
    {
        ValidateUnknownFunctionInputs(n, r);
        return Factorial(n) / (Factorial(r) * Factorial(n - r));
    }

    // Both inferred functions use the same valid input range: 0 <= r <= n <= 20.
    private static void ValidateUnknownFunctionInputs(int n, int r)
    {
        if (n < 0 || n > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        if (r < 0 || r > n)
        {
            throw new ArgumentOutOfRangeException(nameof(r));
        }
    }

    public double DoOperation(double a, double b, string op)
    {
        return op switch
        {
            "a" => Add(a, b),
            "s" => Subtract(a, b),
            "m" => Multiply(a, b),
            "d" => Divide(a, b),
            _ => throw new ArgumentException("Unknown operation.")
        };
    }
}
