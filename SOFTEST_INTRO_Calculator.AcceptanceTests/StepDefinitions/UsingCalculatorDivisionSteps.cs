using Reqnroll; 
using SOFTEST_INTRO_Calculator.AcceptanceTests.Support; 
namespace SOFTEST_INTRO_Calculator.AcceptanceTests.StepDefinitions; 
[Binding] 
public sealed class UsingCalculatorDivisionSteps 
{ 
    private readonly CalculatorContext _context; 
    public UsingCalculatorDivisionSteps(CalculatorContext context) => _context = context; 
    [When("I have entered {double} and {double} into the calculator and press divide")] 
    public void WhenIHaveEnteredAndPressDivide(double numerator, double divisor) 
    { 
        _context.Result = null; 
        _context.Error = null; 
        
        try 
        { 
            _context.Result = _context.Calculator.Divide(numerator, divisor); 
        } 
        catch (ArgumentException error) 
        { 
            _context.Error = error; 
        } 
    } 
}