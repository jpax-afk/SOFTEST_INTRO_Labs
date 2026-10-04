using Microsoft.AspNetCore.Mvc.Testing;

namespace SOFTEST_INTRO_Calculator.WebUiTests;

internal sealed class CalculatorApplicationHost : IAsyncDisposable
{
    private WebApplicationFactory<Program>? _ownedApplication;
    private HttpClient? _readinessClient;

    public string BaseUrl { get; private set; } = string.Empty;

    public async Task StartAsync()
    {
        string? configuredUrl = Environment.GetEnvironmentVariable(
            "SOFTEST_INTRO_BASE_URL");

        if (!string.IsNullOrWhiteSpace(configuredUrl))
        {
            if (!Uri.TryCreate(
                    configuredUrl, UriKind.Absolute, out Uri? address)
                || (address.Scheme != Uri.UriSchemeHttp
                    && address.Scheme != Uri.UriSchemeHttps))
            {
                throw new InvalidOperationException(
                    "SOFTEST_INTRO_BASE_URL must be an absolute " +
                    "HTTP or HTTPS URL.");
            }

            BaseUrl = address.ToString().TrimEnd('/');
            _readinessClient = new HttpClient
            {
                BaseAddress = address,
                Timeout = TimeSpan.FromSeconds(1)
            };
        }
        else
        {
            _ownedApplication = new WebApplicationFactory<Program>();
            _ownedApplication.UseKestrel(0);
            _readinessClient = _ownedApplication.CreateClient();
            _readinessClient.Timeout = TimeSpan.FromSeconds(1);
            BaseUrl = _readinessClient.BaseAddress!
                .ToString().TrimEnd('/');
        }

        await RequireReadyAsync();
    }

    private async Task RequireReadyAsync()
    {
        Exception? lastError = null;

        for (int attempt = 1; attempt <= 20; attempt++)
        {
            try
            {
                using HttpResponseMessage response =
                    await _readinessClient!.GetAsync("/");

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                lastError = new HttpRequestException(
                    $"The server returned HTTP {(int)response.StatusCode}.");
            }
            catch (Exception error) when (
                error is HttpRequestException or TaskCanceledException)
            {
                lastError = error;
            }

            await Task.Delay(250);
        }

        throw new InvalidOperationException(
            $"The calculator web application is not reachable at {BaseUrl}. " +
            "Start the configured server or unset " +
            "SOFTEST_INTRO_BASE_URL.",
            lastError);
    }

    public async ValueTask DisposeAsync()
    {
        _readinessClient?.Dispose();

        if (_ownedApplication is not null)
        {
            await _ownedApplication.DisposeAsync();
        }
    }
}
