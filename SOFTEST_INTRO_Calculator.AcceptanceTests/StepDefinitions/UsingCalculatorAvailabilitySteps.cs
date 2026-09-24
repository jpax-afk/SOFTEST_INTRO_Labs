using NUnit.Framework;
using Reqnroll;
using SOFTEST_INTRO_Calculator.AcceptanceTests.Support;

namespace SOFTEST_INTRO_Calculator.AcceptanceTests.StepDefinitions;

[Binding]
public sealed class UsingCalculatorAvailabilitySteps
{
    private readonly CalculatorContext _context;
    private readonly ReliabilityContext _reliability;

    public UsingCalculatorAvailabilitySteps(
        CalculatorContext context,
        ReliabilityContext reliability)
    {
        _context = context;
        _reliability = reliability;
    }

    [Given("the reliability values are")]
    public void GivenTheReliabilityValuesAre(DataTable table)
    {
        var values = table.Rows[0];

        _reliability.Mtbf = double.Parse(values["MTBF"]);
        _reliability.Mttr = double.Parse(values["MTTR"]);
    }

    [When("I have entered {double} and {double} into the calculator and press MTBF")]
    public void WhenICalculateMtbf(
        double operatingTime,
        double numberOfFailures)
    {
        _context.Result = null;
        _context.Error = null;

        try
        {
            _context.Result = _context.Calculator.CalculateMtbf(
                operatingTime,
                numberOfFailures);
        }
        catch (ArgumentException error)
        {
            _context.Error = error;
        }
    }

    [When("I have entered {double} and {double} into the calculator and press Availability")]
    public void WhenICalculateAvailability(
        double mtbf,
        double mttr)
    {
        _context.Result = null;
        _context.Error = null;

        try
        {
            _context.Result =
                _context.Calculator.CalculateAvailability(mtbf, mttr);
        }
        catch (ArgumentException error)
        {
            _context.Error = error;
        }
    }

    // Uses the named MTBF and MTTR values that the Given step read from the table.
    // The calculation remains in Calculator rather than being duplicated here.
    [When("I calculate Availability from these values")]
    public void WhenICalculateAvailabilityFromTheseValues()
    {
        _context.Result = null;
        _context.Error = null;

        try
        {
            _context.Result = _context.Calculator.CalculateAvailability(
                _reliability.Mtbf,
                _reliability.Mttr);
        }
        catch (ArgumentException error)
        {
            _context.Error = error;
        }
    }

    // Both validation failures are stored by their When steps in CalculatorContext.Error.
    [Then("the MTBF calculation should be rejected")]
    public void ThenMtbfShouldBeRejected()
    {
        Assert.That(
            _context.Error,
            Is.InstanceOf<ArgumentException>());
    }

    [Then("the availability calculation should be rejected")]
    public void ThenAvailabilityShouldBeRejected()
    {
        Assert.That(
            _context.Error,
            Is.InstanceOf<ArgumentException>());
    }
}
