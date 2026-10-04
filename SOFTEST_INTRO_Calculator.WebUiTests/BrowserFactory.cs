using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SOFTEST_INTRO_Calculator.WebUiTests;

internal static class BrowserFactory
{
    public static IWebDriver CreateFromEnvironment()
    {
        string browser = (
            Environment.GetEnvironmentVariable(
                "SOFTEST_INTRO_BROWSER") ?? "chrome")
            .Trim()
            .ToLowerInvariant();

        bool headless = string.Equals(
            Environment.GetEnvironmentVariable("HEADLESS"),
            "true", StringComparison.OrdinalIgnoreCase);

        return browser switch
        {
            "chrome" => new ChromeDriver(
                CreateChromeOptions(headless)),
            "firefox" => new FirefoxDriver(
                CreateFirefoxOptions(headless)),
            _ => throw new ArgumentException(
                $"Unsupported browser: {browser}")
        };
    }

//options and closing brace
    private static ChromeOptions CreateChromeOptions(
        bool headless)
    {
        var options = new ChromeOptions();
        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1280,900");
        }
        return options;
    }

    private static FirefoxOptions CreateFirefoxOptions(
        bool headless)
    {
        var options = new FirefoxOptions();
        if (headless)
        {
            options.AddArgument("-headless");
        }
        return options;
    }
}
