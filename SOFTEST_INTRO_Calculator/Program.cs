using System.Globalization;
using SOFTEST_INTRO_Calculator;
var calculator = new Calculator();
Console.WriteLine("Calculator operations:");
Console.WriteLine("a=add, s=subtract, m=multiply, d=divide f=factorial, t=triangle area, c=circle area");
Console.Write("Operation: ");
string op = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
if (op == "f")
{
    Console.Write("Whole number: ");
    string input = Console.ReadLine() ?? "";

    bool isWholeNumber = int.TryParse(
        input,
        NumberStyles.Integer,
        CultureInfo.InvariantCulture,
        out int n);

    if (!isWholeNumber)
    {
        Console.WriteLine("Enter a whole number.");
        return;
    }

    try
    {
        long result = calculator.Factorial(n);
        Console.WriteLine("Result: " + result);
    }
    catch (ArgumentOutOfRangeException error)
    {
        Console.WriteLine(error.Message);
    }

    return;
}
else if (op == "t")
{
    Console.Write("Height: ");
    string heightInput = Console.ReadLine() ?? "";
    Console.Write("Width: ");
    string widthInput = Console.ReadLine() ?? "";

    bool isHeightValid = double.TryParse(
        heightInput,
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out double height);

    bool isWidthValid = double.TryParse(
        widthInput,
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out double width);

    if (!isHeightValid || !isWidthValid ||
        !double.IsFinite(height) || !double.IsFinite(width))
    {
        Console.WriteLine("Enter finite numbers; use . for decimals.");
        return;
    }

    try
    {
        double result = calculator.TriangleArea(height, width);
        string text = result.ToString(CultureInfo.InvariantCulture);
        Console.WriteLine("Result: " + text);
    }
    catch (ArgumentOutOfRangeException error)
    {
        Console.WriteLine(error.Message);
    }

    return;
}
else if (op == "c")
{
    Console.Write("Radius: ");
    string radiusInput = Console.ReadLine() ?? "";

    bool isRadiusValid = double.TryParse(
        radiusInput,
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out double radius);

    if (!isRadiusValid || !double.IsFinite(radius))
    {
        Console.WriteLine("Enter a finite number; use . for decimals.");
        return;
    }

    try
    {
        double result = calculator.CircleArea(radius);
        string text = result.ToString(CultureInfo.InvariantCulture);
        Console.WriteLine("Result: " + text);
    }
    catch (ArgumentOutOfRangeException error)
    {
        Console.WriteLine(error.Message);
    }

    return;
}
else 
{
    Console.Write("First number: ");
    string first = Console.ReadLine() ?? "";
    Console.Write("Second number: ");
    string second = Console.ReadLine() ?? "";
    bool firstOk = double.TryParse(
    first,
    NumberStyles.Float,
    CultureInfo.InvariantCulture,
    out double a);
    bool secondOk = double.TryParse(
    second,
    NumberStyles.Float,
    CultureInfo.InvariantCulture,
    out double b);
    if (!firstOk || !secondOk ||
    !double.IsFinite(a) || !double.IsFinite(b))
    {
    Console.WriteLine("Enter finite numbers; use . for decimals.");
    return;
    }
    try
    {
    double result = calculator.DoOperation(a, b, op);
    string text = result.ToString(CultureInfo.InvariantCulture);
    Console.WriteLine("Result: " + text);
    }
    catch (ArgumentException error)
    {
    Console.WriteLine(error.Message);
    }
}