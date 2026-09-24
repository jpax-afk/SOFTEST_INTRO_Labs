using Reqnroll; 
using SOFTEST_INTRO_Calculator.AcceptanceTests.Support;

namespace SOFTEST_INTRO_Calculator.AcceptanceTests.StepDefinitions; 

[Binding] 
public sealed class UsingCalculatorAdditionSteps 
{ 
    private readonly CalculatorContext _context; 
    public UsingCalculatorAdditionSteps(CalculatorContext context) 
        => _context = context; 
        
    [When("I have entered {double} and {double} into the calculator and press add")] 
    public void WhenIHaveEnteredAndPressAdd(double first, double second) 
    { 
        _context.Result = _context.Calculator.Add(first, second); 
    } 
}