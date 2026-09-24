using NUnit.Framework;
using Reqnroll;
using SOFTEST_INTRO_Calculator.AcceptanceTests.Support;

namespace SOFTEST_INTRO_Calculator.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class UsingCalculatorBasicReliabilitySteps
{
    private readonly CalculatorContext _context;
    private readonly BasicMusaContext _musa;

    // Reqnroll supplies the same context instances to every step in one scenario.
    public UsingCalculatorBasicReliabilitySteps(
        CalculatorContext context,
        BasicMusaContext musa)
    {
        _context = context;
        _musa = musa;
    }

    // These Given steps store all three model inputs explicitly in scenario state.
    [Given("the initial failure intensity is {double} failures per hour")]
    public void GivenTheInitialFailureIntensityIs(double initialFailureIntensity)
    {
        _musa.InitialFailureIntensity = initialFailureIntensity;
    }

    [Given("the expected total number of failures is {double}")]
    public void GivenTheExpectedTotalNumberOfFailuresIs(double expectedTotalFailures)
    {
        _musa.ExpectedTotalFailures = expectedTotalFailures;
    }

    [Given("the accumulated execution time is {double} hours")]
    public void GivenTheAccumulatedExecutionTimeIs(double executionTime)
    {
        _musa.ExecutionTime = executionTime;
    }

    [When("I calculate the current failure intensity")]
    public void WhenICalculateTheCurrentFailureIntensity()
    {
        ExecuteCalculation(() =>
            _context.Calculator.CalculateCurrentFailureIntensity(
                _musa.InitialFailureIntensity,
                _musa.ExpectedTotalFailures,
                _musa.ExecutionTime));
    }

    [When("I calculate the expected cumulative failures")]
    public void WhenICalculateTheExpectedCumulativeFailures()
    {
        ExecuteCalculation(() =>
            _context.Calculator.CalculateExpectedCumulativeFailures(
                _musa.InitialFailureIntensity,
                _musa.ExpectedTotalFailures,
                _musa.ExecutionTime));
    }

    [Then("the Basic Musa calculation should be rejected")]
    public void ThenTheBasicMusaCalculationShouldBeRejected()
    {
        Assert.That(
            _context.Error,
            Is.TypeOf<ArgumentOutOfRangeException>());
    }

    // Both Musa calculations use the same result/error handling.
    private void ExecuteCalculation(Func<double> calculation)
    {
        _context.Result = null;
        _context.Error = null;

        try
        {
            _context.Result = calculation();
        }
        catch (ArgumentOutOfRangeException error)
        {
            _context.Error = error;
        }
    }
}
