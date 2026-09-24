using System.Globalization;

namespace SOFTEST_INTRO_Calculator;
public class Calculator
{
 public double Add(double a, double b)
 {
     // Lab 2's special examples join binary-digit operands, then interpret the
     // joined digits as a binary number: 10 and 11 become 1011, which is 11.
     if (ContainsOnlyBinaryDigits(a) && ContainsOnlyBinaryDigits(b))
     {
         string binaryDigits =
             a.ToString("0", CultureInfo.InvariantCulture) +
             b.ToString("0", CultureInfo.InvariantCulture);

         return Convert.ToInt64(binaryDigits, 2);
     }

     // All other inputs retain the original arithmetic-addition behaviour.
     return a + b;
 }

 private static bool ContainsOnlyBinaryDigits(double value)
 {
     if (value < 0 || value != Math.Truncate(value))
     {
         return false;
     }

     string digits = value.ToString("0", CultureInfo.InvariantCulture);

     foreach (char digit in digits)
     {
         if (digit != '0' && digit != '1')
         {
             return false;
         }
     }

     return true;
 }
 public double Subtract(double a, double b) => a - b;
 public double Multiply(double a, double b) => a * b;
 // Starter version: complete the zero-divisor rule in section 5.
 public double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new ArgumentException("Throws ArgumentException");
        }
        else
            {
                return a / b;
            }
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
        else
        {
            long result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
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
        else
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
        else
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

// mtbf calculation
public double CalculateMtbf(double operatingTime, double numberofFailures)
    {
        // Both inputs must be strictly positive under the Lab 2 contract.
        if (operatingTime <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(operatingTime));
        }

        if (numberofFailures <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberofFailures));
        }

        return operatingTime / numberofFailures;
    }

// availability calculation
public double CalculateAvailability(double operatingTime, double downtime)
    {
        if (operatingTime < 0 || downtime < 0)
        {
            throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException");
        }
        else if (operatingTime == 0 && downtime == 0)
        {
            throw new ArgumentException("Throws ArgumentException");
        }
        else
        {
            return operatingTime / (operatingTime + downtime);
        }
    }

// Current failure intensity: lambda(t) = lambda0 * exp(-lambda0 * t / nu0).
public double CalculateCurrentFailureIntensity(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime)
{
    ValidateBasicMusaInputs(
        initialFailureIntensity,
        expectedTotalFailures,
        executionTime);

    return initialFailureIntensity * Math.Exp(
        -initialFailureIntensity * executionTime
        / expectedTotalFailures);
}

// Expected cumulative failures: mu(t) = nu0 * (1 - exp(-lambda0 * t / nu0)).
public double CalculateExpectedCumulativeFailures(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime)
{
    ValidateBasicMusaInputs(
        initialFailureIntensity,
        expectedTotalFailures,
        executionTime);

    return expectedTotalFailures * (
        1 - Math.Exp(
            -initialFailureIntensity * executionTime
            / expectedTotalFailures));
}

// Validate inputs for Basic Musa model calculations
private static void ValidateBasicMusaInputs(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime)
{
    if (initialFailureIntensity <= 0)
    {
        throw new ArgumentOutOfRangeException(
            nameof(initialFailureIntensity));
    }

    if (expectedTotalFailures <= 0)
    {
        throw new ArgumentOutOfRangeException(
            nameof(expectedTotalFailures));
    }

    if (executionTime < 0)
    {
        throw new ArgumentOutOfRangeException(
            nameof(executionTime));
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
