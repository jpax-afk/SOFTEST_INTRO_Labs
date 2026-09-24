using SOFTEST_INTRO_Calculator;
using NUnit.Framework;

namespace SOFTEST_INTRO_Calculator.UnitTests;

public class CalculatorTestLab1
{
    private CalculatorLab1 _calculator = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new CalculatorLab1();
    }

    [Test]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange: the calculator is created in SetUp.
        // Act
        double result = _calculator.Add(10, 20);
        // Assert
        Assert.That(result, Is.EqualTo(30));
    }

    [TestCase(0, 0, 0)]
    [TestCase(0, 5, 5)]
    [TestCase(-3, 8, 5)]
    [TestCase(0.1, 0.2, 0.3)]
    public void Add_RepresentativeInputs_ReturnsSum(
        double a, double b, double expected)
    {
        double result = _calculator.Add(a, b);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Subtraction
    [TestCase(0, 12, -12)]
    [TestCase(5, 3, 2)]
    [TestCase(1.5, 0.8, 0.7)]
    [TestCase(-0.7, -0.9, 0.2)]
    public void Subtract_RepresentativeInputs_ReturnsValue(
        double a, double b, double expected)
    {
        double result = _calculator.Subtract(a, b);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Multiplication
    [TestCase(0, 12, 0)]
    [TestCase(5, 3, 15)]
    [TestCase(1.5, 0.8, 1.2)]
    [TestCase(-0.7, -0.9, 0.63)]
    public void Multiply_RepresentativeInputs_ReturnsValue(
        double a, double b, double expected)
    {
        double result = _calculator.Multiply(a, b);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Division
    [TestCase(1, 2, 0.5)]
    [TestCase(0, 15, 0)]
    [TestCase(15, -3, -5)]
    public void Divide_ValidInputs_ReturnQuotion(
        double a, double b, double expected)
    {
        double result = _calculator.Divide(a, b);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    [TestCase(15, 0)]
    [TestCase(0, 0)]
    public void Divide_ZeroDivisor_ThrowsArgumentException(double a, double b)
    {
        Assert.That(() => _calculator.Divide(a, b),
            Throws.TypeOf<ArgumentException>());
    }

    // Factorial Test input = 0, 1, 5, 20
    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(5, 120)]
    [TestCase(20, 2432902008176640000)]
    public void Factorial_ReturnsCorrectValue(int n, long expected)
    {
        long result = _calculator.Factorial(n);
        Assert.That(result, Is.EqualTo(expected));
    }

    // Factorial Test input = -1, 21
    [TestCase(-1)]
    [TestCase(21)]
    public void Factorial_InvalidInput_ThrowsArgumentOutOfRangeException(int n)
    {
        Assert.That(() => _calculator.Factorial(n),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    // Triangle area tests, correct input/output
    [TestCase(3, 4, 6)]
    [TestCase(0, 4, 0)]
    [TestCase(3, 0, 0)]
    public void TriangleArea_ValidInputs_ReturnsArea(
        double height, double width, double expected)
    {
        double result = _calculator.TriangleArea(height, width);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Triangle area tests, negative input
    [TestCase(-3, 4)]
    [TestCase(3, -4)]
    public void TriangleArea_NegativeInput_ThrowsArgumentOutOfRangeException(
        double height, double width)
    {
        Assert.That(() => _calculator.TriangleArea(height, width),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    // Circle area tests, correct input/output
    [TestCase(1)]
    [TestCase(0)]
    public void CircleArea_ValidInput_ReturnsArea(double radius)
    {
        double expected = Math.PI * radius * radius;
        double result = _calculator.CircleArea(radius);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Circle area tests, negative input
    [TestCase(-5)]
    public void CircleArea_NegativeInput_ThrowsArgumentOutOfRangeException(
        double radius)
    {
        Assert.That(() => _calculator.CircleArea(radius),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    // Function A represents permutations (nPr).
    // The (6, 2) case is an extra discriminating example: it gives 30, not 15.
    [TestCase(5, 5, 120)]
    [TestCase(5, 4, 120)]
    [TestCase(5, 3, 60)]
    [TestCase(5, 0, 1)]
    [TestCase(0, 0, 1)]
    [TestCase(6, 2, 30)]
    public void UnknownFunctionA_ValidInputs_ReturnsPermutations(
        int n, int r, long expected)
    {
        long result = _calculator.UnknownFunctionA(n, r);
        Assert.That(result, Is.EqualTo(expected));
    }

    // Function B represents combinations (nCr).
    // The (6, 2) case distinguishes it from Function A because it gives 15, not 30.
    [TestCase(5, 5, 1)]
    [TestCase(5, 4, 5)]
    [TestCase(5, 3, 10)]
    [TestCase(5, 0, 1)]
    [TestCase(0, 0, 1)]
    [TestCase(6, 2, 15)]
    public void UnknownFunctionB_ValidInputs_ReturnsCombinations(
        int n, int r, long expected)
    {
        long result = _calculator.UnknownFunctionB(n, r);
        Assert.That(result, Is.EqualTo(expected));
    }

    // These cases cover r > n, negative n, negative r, and n above 20.
    [TestCase(-4, 5)]
    [TestCase(4, 5)]
    [TestCase(-1, 0)]
    [TestCase(5, -1)]
    [TestCase(21, 1)]
    public void UnknownFunctionA_InvalidInputs_ThrowsArgumentOutOfRangeException(
        int n, int r)
    {
        Assert.That(() => _calculator.UnknownFunctionA(n, r),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [TestCase(-4, 5)]
    [TestCase(4, 5)]
    [TestCase(-1, 0)]
    [TestCase(5, -1)]
    [TestCase(21, 1)]
    public void UnknownFunctionB_InvalidInputs_ThrowsArgumentOutOfRangeException(
        int n, int r)
    {
        Assert.That(() => _calculator.UnknownFunctionB(n, r),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    // A finite set of passing examples cannot uniquely determine an implementation:
    // another formula or a lookup table could produce the same tested outputs while
    // behaving differently for inputs that were not tested.
}
