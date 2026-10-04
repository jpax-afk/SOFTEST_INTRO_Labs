using OpenQA.Selenium;

namespace SOFTEST_INTRO_Calculator.WebUiTests;

[TestFixture]
[Category("Browser")]
public sealed class CalculatorBrowserTests
{
    private CalculatorApplicationHost _application = null!;
    private IWebDriver? _driver;
    private CalculatorPage _page = null!;

    [OneTimeSetUp]
    public async Task StartApplication()
    {
        _application = new CalculatorApplicationHost();
        await _application.StartAsync();
    }

    [SetUp]
    public void SetUp()
    {
        _driver = BrowserFactory.CreateFromEnvironment();
        _page = new CalculatorPage(
            _driver, _application.BaseUrl);
        _page.Open();
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    [OneTimeTearDown]
    public async Task StopApplication()
    {
        await _application.DisposeAsync();
    }

    [Test]
    public void Add_TwoAndThree_ShowsFive()
    {
        _page.Calculate("2", "3", "a");

        string result = _page.WaitForResult();

        Assert.That(result, Is.EqualTo("5"));
    }

    [Test]
    public void Divide_ByZero_ShowsRejection()
    {
        _page.Calculate("10", "0", "d");

        string error = _page.WaitForError();

        Assert.That(
            error,
            Is.EqualTo("The calculation was rejected."));
    }
}
