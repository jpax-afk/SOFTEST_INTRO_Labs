using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SOFTEST_INTRO_Calculator.WebUiTests;

public sealed class CalculatorPage
{
    private static readonly By FirstNumber =
        By.CssSelector("[data-testid='first-number']");
    private static readonly By SecondNumber =
        By.CssSelector("[data-testid='second-number']");
    private static readonly By Operation =
        By.CssSelector("[data-testid='operation']");
    private static readonly By CalculateButton =
        By.CssSelector("[data-testid='calculate']");
    private static readonly By Result =
        By.CssSelector("[data-testid='result']");
    private static readonly By Error =
        By.CssSelector("[data-testid='error']");

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    public CalculatorPage(IWebDriver driver, string baseUrl)
    {
        _driver = driver;
        _baseUrl = baseUrl.TrimEnd('/');
        _wait = new WebDriverWait(
            driver, TimeSpan.FromSeconds(5));
    }

    //navigation, action and result
    public void Open()
    {
        _driver.Navigate().GoToUrl(_baseUrl);
        _wait.Until(driver =>
            driver.Title == "SOFTEST Calculator");
    }

    public void Calculate(
        string first, string second, string operation)
    {
        Enter(FirstNumber, first);
        Enter(SecondNumber, second);

        var operations = new SelectElement(
            _driver.FindElement(Operation));
        operations.SelectByValue(operation);

        _driver.FindElement(CalculateButton).Click();
    }

    public string WaitForResult()
    {
        _wait.Until(driver =>
            !string.IsNullOrWhiteSpace(
                driver.FindElement(Result).Text));
        return _driver.FindElement(Result).Text;
    }
    
    // error, input helper and closing brace
    public string WaitForError()
    {
        _wait.Until(driver =>
            !string.IsNullOrWhiteSpace(
                driver.FindElement(Error).Text));
        return _driver.FindElement(Error).Text;
    }

    private void Enter(By locator, string value)
    {
        IWebElement element = _driver.FindElement(locator);
        element.Clear();
        element.SendKeys(value);
    }
}
