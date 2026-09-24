using SOFTEST_INTRO_Calculator;
using NUnit.Framework;
namespace SOFTEST_INTRO_Calculator.UnitTests;
public class CalculatorTests
{
 private Calculator _calculator = null!;
 [SetUp]
 public void SetUp()
 {
 _calculator = new Calculator();
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
    public void TriangleArea_ValidInputs_ReturnsArea(double height, double width, double expected)
    {
        double result = _calculator.TriangleArea(height, width);
        Assert.That(result, Is.EqualTo(expected).Within(1e-9));
    }

    // Triangle area tests, negative input
    [TestCase(-3, 4)]
    [TestCase(3, -4)]
    public void TriangleArea_NegativeInput_ThrowsArgumentOutOfRangeException(double height, double width)
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
    public void CircleArea_NegativeInput_ThrowsArgumentOutOfRangeException(double radius)
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


// mtbf calculate test
// proper input for mtbf
[TestCase(1000, 5, 200)]
[TestCase(90, 3, 30)]
public void CalculateMtbf_ValidInputs_ReturnsExpectedResult(
    double operatingTime,
    double numberOfFailures,
    double expected)
{
    double result = _calculator.CalculateMtbf(
        operatingTime,
        numberOfFailures);

    Assert.That(result, Is.EqualTo(expected).Within(1e-9));
}

// mtbf inproper input
[TestCase(0, 5)]
[TestCase(-1, 5)]
[TestCase(1000, 0)]
[TestCase(1000, -1)]
public void CalculateMtbf_InvalidInput_ThrowsArgumentOutOfRangeException(
    double operatingTime,
    double numberOfFailures)
{
    Assert.That(
        () => _calculator.CalculateMtbf(
            operatingTime,
            numberOfFailures),
        Throws.TypeOf<ArgumentOutOfRangeException>());
}

// proper input for for avaiblability calculation

[TestCase(200, 50, 0.8)]
[TestCase(0, 10, 0)]
[TestCase(10, 0, 1)]
public void CalculateAvailability_ValidInputs_ReturnsExpectedRatio(
    double mtbf,
    double mttr,
    double expected)
{
    double result = _calculator.CalculateAvailability(mtbf, mttr);

    Assert.That(result, Is.EqualTo(expected).Within(1e-9));
}

// improper input for avaiblability calculation

[TestCase(-1, 10)]
[TestCase(10, -1)]
[TestCase(0, 0)]
public void CalculateAvailability_InvalidInputs_ThrowsArgumentException(
    double mtbf,
    double mttr)
{
    Assert.That(
        () => _calculator.CalculateAvailability(mtbf, mttr),
        Throws.InstanceOf<ArgumentException>());
}

// At zero execution time the current intensity equals its initial value.
// The normal case independently uses 10 * exp(-0.5).
[TestCase(10, 100, 0, 10)]
[TestCase(10, 100, 5, 6.065306597126334)]
public void CalculateCurrentFailureIntensity_ValidInputs_ReturnsExpectedResult(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime,
    double expected)
{
    double result = _calculator.CalculateCurrentFailureIntensity(
        initialFailureIntensity,
        expectedTotalFailures,
        executionTime);

    Assert.That(result, Is.EqualTo(expected).Within(1e-9));
}

// At zero execution time no failures have accumulated.
// The normal case independently uses 100 * (1 - exp(-0.5)).
[TestCase(10, 100, 0, 0)]
[TestCase(10, 100, 5, 39.346934028736655)]
public void CalculateExpectedCumulativeFailures_ValidInputs_ReturnsExpectedResult(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime,
    double expected)
{
    double result = _calculator.CalculateExpectedCumulativeFailures(
        initialFailureIntensity,
        expectedTotalFailures,
        executionTime);

    Assert.That(result, Is.EqualTo(expected).Within(1e-9));
}

// Boundary cases: lambda0 and nu0 must be greater than zero, while time may
// equal zero but cannot be negative.
[TestCase(0, 100, 5)]
[TestCase(10, 0, 5)]
[TestCase(10, 100, -1)]
public void CalculateCurrentFailureIntensity_InvalidInput_ThrowsArgumentOutOfRangeException(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime)
{
    Assert.That(
        () => _calculator.CalculateCurrentFailureIntensity(
            initialFailureIntensity,
            expectedTotalFailures,
            executionTime),
        Throws.TypeOf<ArgumentOutOfRangeException>());
}

[TestCase(0, 100, 5)]
[TestCase(10, 0, 5)]
[TestCase(10, 100, -1)]
public void CalculateExpectedCumulativeFailures_InvalidInput_ThrowsArgumentOutOfRangeException(
    double initialFailureIntensity,
    double expectedTotalFailures,
    double executionTime)
{
    Assert.That(
        () => _calculator.CalculateExpectedCumulativeFailures(
            initialFailureIntensity,
            expectedTotalFailures,
            executionTime),
        Throws.TypeOf<ArgumentOutOfRangeException>());
}
}
