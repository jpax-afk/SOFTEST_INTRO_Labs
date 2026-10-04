using SOFTEST_INTRO_Calculator;

const string page = """
    <!doctype html>
    <html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>SOFTEST Calculator</title>
        <style>
            body {
                font-family: system-ui, sans-serif;
                margin: 2rem;
            }

            main {
                max-width: 32rem;
            }

            label {
                display: block;
                margin-top: 0.8rem;
            }

            input,
            select,
            button {
                font: inherit;
                padding: 0.45rem;
            }

            button {
                display: block;
                margin-top: 1rem;
            }

            [data-testid="error"] {
                color: #a40000;
            }
        </style>
    </head>
    <body>
        <main>
            <h1>Calculator</h1>

            <label for="first-number">First number</label>
            <input id="first-number" data-testid="first-number" type="number" step="any">

            <label for="second-number">Second number</label>
            <input id="second-number" data-testid="second-number" type="number" step="any">

            <label for="operation">Operation</label>
            <select id="operation" data-testid="operation">
                <option value="a">Add</option>
                <option value="s">Subtract</option>
                <option value="m">Multiply</option>
                <option value="d">Divide</option>
            </select>

            <button type="button" data-testid="calculate">Calculate</button>

            <p>
                <strong>Result:</strong>
                <span data-testid="result" aria-live="polite"></span>
            </p>
            <p data-testid="error" aria-live="assertive"></p>
        </main>

        <script>
            const get = testId => document.querySelector(`[data-testid="${testId}"]`);

            get("calculate").addEventListener("click", async () => {
                get("result").textContent = "";
                get("error").textContent = "";

                const query = new URLSearchParams({
                    first: get("first-number").value,
                    second: get("second-number").value,
                    operation: get("operation").value
                });

                const response = await fetch(`/calculate?${query}`);
                const data = await response.json();

                if (response.ok) {
                    get("result").textContent = data.result;
                } else {
                    get("error").textContent =
                        data.error ?? "The calculation could not be completed.";
                }
            });
        </script>
    </body>
    </html>
    """;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content(page, "text/html"));

app.MapGet(
    "/calculate",
    async (double first, double second, string operation) =>
    {
        await Task.Delay(300);
        return Calculate(first, second, operation);
    });

app.Run();

static IResult Calculate(
    double first,
    double second,
    string operation)
{
    try
    {
        var calculator = new Calculator();
        double result = calculator.DoOperation(first, second, operation);

        return Results.Ok(new { result });
    }
    catch (ArgumentException)
    {
        return Results.BadRequest(new
        {
            error = "The calculation was rejected."
        });
    }
}

public partial class Program { }
