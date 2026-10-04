# Lab 3: Continuous Integration and Testing Dependencies

This README explains the main ideas and implementation completed in Lab 3. It is intended to be both a project guide and a revision resource.

The lab has two main goals:

1. Run the complete test suite automatically with GitHub Actions.
2. Make code with a file-system dependency easier to test using an interface, Dependency Injection, and Moq.

## Project structure

```text
SOFTEST_INTRO_Labs/
├── .github/workflows/dotnet.yml
├── SOFTEST_INTRO_Calculator/
├── SOFTEST_INTRO_Calculator.UnitTests/
├── SOFTEST_INTRO_Calculator.AcceptanceTests/
├── SOFTEST_INTRO_Calculator.IntegrationTests/
└── SOFTEST_INTRO_Labs.sln
```

| Project | Purpose |
|---|---|
| `SOFTEST_INTRO_Calculator` | Production calculator code |
| `SOFTEST_INTRO_Calculator.UnitTests` | Fast, isolated NUnit tests, including Moq tests |
| `SOFTEST_INTRO_Calculator.AcceptanceTests` | Reqnroll scenarios describing user-level behaviour |
| `SOFTEST_INTRO_Calculator.IntegrationTests` | Tests involving the real `FileReader` and file system |

## Part I: Continuous Integration

### Git, CI, automated testing, and regression testing

These terms are related, but they do not mean the same thing.

| Concept | Meaning in this lab |
|---|---|
| Git | Records versions of the source code and supports collaboration |
| GitHub | Stores and shares the Git repository remotely |
| Continuous Integration (CI) | Automatically restores, builds, and tests repository changes |
| Automated testing | Executes predefined checks without manually repeating every action |
| Regression testing | Repeats existing tests after a change to detect broken behaviour |

The GitHub Actions workflow is triggered by pushes and pull requests:

```yaml
on:
  push:
  pull_request:
```

The workflow performs this sequence:

```text
Push or pull request
        ↓
Start a clean Ubuntu runner
        ↓
Check out the repository
        ↓
Install .NET 10
        ↓
Restore NuGet dependencies
        ↓
Build the solution
        ↓
Run all test projects
        ↓
Report success or failure
```

### Why use a clean GitHub runner?

A local machine may already contain packages, files, environment settings, or cached build output that accidentally help the project work. A clean runner checks whether the committed repository and declared dependencies are sufficient by themselves.

It can expose:

- files that were never committed;
- undeclared packages or dependencies;
- hard-coded local paths;
- Windows-specific assumptions;
- filename capitalisation errors;
- incorrect working-directory assumptions; and
- tests that are not actually being discovered.

### The intentional failing test

The expected value was temporarily changed from:

```csharp
[TestCase(0, 5, 5)]
```

to:

```csharp
[TestCase(0, 5, 9)]
```

The calculator returned `5`, but the test expected `9`, so the GitHub Actions workflow failed. This demonstrated that CI can automatically detect a disagreement between the implementation and the configured regression tests.

CI did not decide whether the code or expectation was correct. Requirements and agreed examples are still needed to make that decision.

### What does a green workflow prove?

A green workflow provides evidence that:

- the repository was checked out successfully;
- dependencies were restored;
- the solution compiled; and
- the configured tests were discovered and passed.

It does **not** prove that the software contains no defects, that every important case was tested, or that the expectations correctly represent the customer's needs.

## Part II: Testing dependencies

### What is a dependency?

A dependency is something that code needs to perform its work. Examples include:

- another class;
- a file system;
- a database;
- a web service;
- a clock;
- a random-number generator; or
- a message queue.

`GenMagicNum` needs file contents, so a file reader is one of its dependencies.

The method selects a number and returns twice its magnitude:

```text
"42" → 42 → 84
"-7" → -7 → absolute value 7 → 14
```

### The original tightly coupled design

The first version created its own reader:

```csharp
var fileReader = new FileReader();
string[] magicStrings = fileReader.Read(path);
```

`FileReader` is a class. `new FileReader()` calls its constructor and creates a `FileReader` object. `fileReader` is the local variable referring to that object.

The problem is not the use of `var`. The problem is that `GenMagicNum` constructs the concrete dependency internally.

The caller can choose the file path:

```csharp
calculator.GenMagicNum(0, "MagicNumbers.txt");
```

However, the caller cannot choose how the content is obtained. The method always creates and uses a real `FileReader`.

```text
Caller chooses:     choice and path
Calculator chooses: new FileReader()
```

This is called **tight coupling** because the calculator logic is directly tied to that concrete reader. Testing the calculation also requires the real file-reading path.

### Why the first tests were integration tests

The original tests involved three real components:

```text
Calculator → FileReader → operating-system file system
```

They created a temporary file, wrote known values into it, asked the calculator to read it, and deleted it after the test. Because component boundaries were crossed, these were integration tests rather than isolated unit tests.

The temporary-file approach remains portable:

```csharp
_path = Path.GetTempFileName();
File.WriteAllLines(_path, new[] { "42", "-7" });
```

The file is removed during teardown:

```csharp
if (File.Exists(_path))
{
    File.Delete(_path);
}
```

## `IFileReader`: the interface contract

`IFileReader` is an interface:

```csharp
public interface IFileReader
{
    string[] Read(string path);
}
```

It says:

> Any supplied implementation must provide a `Read` method that accepts a path and returns an array of strings.

The interface describes **what must be possible**, not **how it must happen**.

The real implementation is:

```csharp
public class FileReader : IFileReader
{
    public string[] Read(string path)
    {
        return File.ReadAllLines(path);
    }
}
```

`FileReader : IFileReader` means that `FileReader` implements the interface and promises to follow its contract.

The `I` prefix is the normal C# naming convention for an interface.

### Understanding the variable and object

```csharp
IFileReader fileReader = new FileReader();
```

| Part | Meaning |
|---|---|
| `IFileReader` | Declared variable type and required contract |
| `fileReader` | Variable name |
| `new FileReader()` | Actual object created at runtime |

An interface is not normally instantiated directly. An `IFileReader` variable can refer to any object whose class implements that interface.

For example, it can refer to either:

```csharp
IFileReader realReader = new FileReader();
```

or a Moq-generated test object:

```csharp
IFileReader testReader = mockFileReader.Object;
```

Through the `IFileReader` variable, the calculator knows that it can call:

```csharp
fileReader.Read(path);
```

## Dependency Injection

Dependency Injection means supplying a dependency from outside the code that uses it instead of constructing it internally.

The revised method receives its reader through a parameter:

```csharp
public double GenMagicNum(
    int choice,
    string path,
    IFileReader fileReader)
{
    ArgumentNullException.ThrowIfNull(fileReader);

    if (choice < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(choice));
    }

    string[] magicStrings = fileReader.Read(path);

    if (choice >= magicStrings.Length)
    {
        throw new ArgumentOutOfRangeException(nameof(choice));
    }

    double magicNumber = double.Parse(magicStrings[choice]);
    return 2 * Math.Abs(magicNumber);
}
```

This is **method/parameter injection** because the dependency is passed into the method.

Before Dependency Injection:

```text
Caller chooses the path
Calculator creates and chooses the reader
```

After Dependency Injection:

```text
Caller chooses the path and supplies the reader
Calculator only uses the supplied IFileReader contract
```

Interfaces are useful for Dependency Injection because they allow implementations to be exchanged, but Dependency Injection does not require an interface and is not limited to one dependency.

## Integration testing after Dependency Injection

The integration test supplies the real implementation:

```csharp
private IFileReader _fileReader = null!;

[SetUp]
public void SetUp()
{
    _calculator = new Calculator();
    _fileReader = new FileReader();
}
```

It passes that real reader to the method:

```csharp
double result = _calculator.GenMagicNum(
    choice,
    _path,
    _fileReader);
```

The design is loosely coupled, but this particular test still deliberately integrates with the real file system.

## Isolated unit testing with Moq

Moq is installed only in the UnitTests project. It should not be added to the production Calculator project.

The unit test creates a controlled `IFileReader` test double:

```csharp
_fileReader = new Mock<IFileReader>();

_fileReader
    .Setup(reader => reader.Read("MagicNumbers.txt"))
    .Returns(new[] { "42", "-7" });
```

This means:

> When `Read("MagicNumbers.txt")` is called, return the controlled values `"42"` and `"-7"`.

No real `MagicNumbers.txt` file is created or read. The calculation still occurs inside `Calculator`; only the reader's response is controlled.

### Result tests

```csharp
[TestCase(0, 84)]
[TestCase(1, 14)]
public void GenMagicNum_ConfiguredValues_ReturnsTwiceMagnitude(
    int choice, double expected)
{
    double result = _calculator.GenMagicNum(
        choice, "MagicNumbers.txt", _fileReader.Object);

    Assert.That(result, Is.EqualTo(expected));
}
```

These tests ask whether the calculator produces the correct result from controlled dependency data.

### Rejection tests

```csharp
[TestCase(-1)]
[TestCase(2)]
public void GenMagicNum_UnsupportedIndex_ThrowsArgumentOutOfRangeException(
    int choice)
{
    Assert.That(
        () => _calculator.GenMagicNum(
            choice, "MagicNumbers.txt", _fileReader.Object),
        Throws.TypeOf<ArgumentOutOfRangeException>());
}
```

These tests check that unsupported indexes are rejected.

### Interaction test

```csharp
[Test]
public void GenMagicNum_ValidChoice_ReadsSuppliedPathOnce()
{
    _calculator.GenMagicNum(
        0, "MagicNumbers.txt", _fileReader.Object);

    _fileReader.Verify(
        reader => reader.Read("MagicNumbers.txt"),
        Times.Once());
}
```

This test checks that the dependency was called with the expected path exactly once.

## Stub role versus mock role

A **test double** is an object used instead of a real collaborator during testing. Its role depends on how a particular test uses it.

### Stub role

A stub supplies predetermined information:

```csharp
.Setup(reader => reader.Read("MagicNumbers.txt"))
.Returns(new[] { "42", "-7" });
```

The test then asserts the calculator's result. The important question is:

> Did the calculator produce the correct result from the controlled values?

### Mock role

A mock records interactions so they can be verified:

```csharp
_fileReader.Verify(
    reader => reader.Read("MagicNumbers.txt"),
    Times.Once());
```

The important question is:

> Did the calculator use its dependency in the expected way?

The same object created with `Mock<IFileReader>` can play either role:

- it plays a **stub** role when it provides controlled return values;
- it plays a **mock** role when the test verifies its interactions.

The library name `Moq` does not automatically determine the test-double role.

## Other test-double roles

| Role | Purpose |
|---|---|
| Dummy | Fills a required parameter but is not used by the tested behaviour |
| Stub | Returns predetermined data to control an indirect input |
| Fake | Provides a simplified but working implementation, such as an in-memory repository |
| Mock | Records interactions so the test can verify how it was used |

## Driver versus stub in incomplete integration

Suppose a component normally sits between a caller and a dependency:

```text
Caller → Component → Dependency
```

A **driver** replaces a missing caller and invokes the component under test:

```text
Driver → Component under test
```

A **stub** replaces a missing called dependency and responds to the component:

```text
Component under test → Stub
```

They can appear together:

```text
Driver → Component under test → Stub
```

A useful memory aid is:

- the driver **drives** the test by making calls;
- the stub **stands in** for something being called.

## Why retain both unit and integration tests?

The two layers may use the same input values, but they provide different evidence.

| Test layer | Dependency used | Evidence provided |
|---|---|---|
| Unit test | Moq `IFileReader` | Calculator logic handles controlled dependency responses correctly |
| Integration test | Real `FileReader` and temporary file | Calculator, reader, and operating-system file system work together |
| Acceptance test | Reqnroll scenarios | Behaviour matches readable user-level examples |
| CI workflow | Clean GitHub runner | The committed solution restores, builds, and runs its configured tests in a clean environment |

A passing unit test does not prove that real files can be read. A passing integration test does not isolate the calculator from file-system problems. Retaining both provides complementary evidence.

## Running the project checks

Run these commands from the folder containing `SOFTEST_INTRO_Labs.sln`:

```powershell
dotnet build SOFTEST_INTRO_Labs.sln
dotnet test SOFTEST_INTRO_Calculator.UnitTests
dotnet test SOFTEST_INTRO_Calculator.IntegrationTests
dotnet test SOFTEST_INTRO_Calculator.AcceptanceTests
dotnet test SOFTEST_INTRO_Labs.sln
```

Always inspect the test counts. A successful command that discovers zero tests provides little behavioural evidence.

## Diagnosing local-pass/CI-fail problems

If a test passes on Windows but fails on the Ubuntu runner, inspect the GitHub Actions log for:

1. the failing workflow step;
2. the failing project and test;
3. the exception message;
4. the stack trace and source line; and
5. the exact path the test attempted to use.

Then investigate:

- filename capitalisation, because Linux paths are case-sensitive;
- Windows-only `\` separators;
- hard-coded absolute paths;
- current-working-directory assumptions;
- files that were never committed;
- files that were not copied to the build output;
- file permissions;
- line-ending differences; and
- differences in SDK or package versions.

Prefer portable APIs such as:

```csharp
Path.Combine(...)
Path.GetTempFileName()
```

Do not disable a failing test merely because it exposes a platform assumption.

## Test automation limitations

Automated testing reduces repeated manual effort and provides fast feedback, but it has limitations:

- automation only checks what it was programmed to check;
- a test can pass while following an incorrect requirement;
- automation cannot repair unclear requirements or weak design by itself;
- writing the first automated test may take longer than one manual check;
- not every subjective or unstable check is worth automating; and
- automated tests require maintenance when requirements or interfaces change.

Tests should therefore be readable, focused, reviewed, and maintained like production code.

## Review questions and answers

### Part I

#### 1. How do Git, CI, automated testing, and regression testing differ?

Git records and shares versions of source code. CI automatically integrates and checks repository changes. Automated testing executes predefined checks without a person repeating each action. Regression testing reruns relevant existing tests after a change to detect damage to previously working behaviour.

#### 2. Why does a successful build without discovered tests provide weak evidence?

It only shows that the code compiled. Tests might be absent, misconfigured, excluded, or undiscovered, so no behavioural checks may have run. The CI log should confirm the number of discovered and executed tests.

#### 3. Why can an automated test pass even when the software does not meet the customer's needs?

The expected result may come from an incorrect or incomplete requirement. The test and implementation can agree with each other while both fail to represent the customer's actual need.

#### 4. What maintenance cost may cause an automated test to lose value over time?

If requirements, interfaces, data, or behaviour change frequently, brittle tests may require continual rewriting. When maintenance effort becomes greater than the benefit of repeating the test, its value decreases.

#### 5. What did the clean GitHub-hosted runner check that the local machine did not?

It checked that the committed repository could restore, build, and test without relying on local files, installed dependencies, cached output, machine-specific paths, or Windows-only assumptions.

### Part II

#### 1. Why were the first `GenMagicNum` tests integration tests rather than isolated unit tests?

They exercised the Calculator, real FileReader, and operating-system file system together through a real temporary file. More than one component boundary was involved.

#### 2. What coupling was created when `GenMagicNum` constructed `FileReader` internally?

The calculator became tied to the concrete `FileReader`. A caller could choose the path but could not replace the content provider with a controlled implementation, so testing the calculation also required the real file-reading path.

#### 3. How did `IFileReader` and parameter injection change the design?

`IFileReader` introduced a contract for reading lines, while parameter injection moved responsibility for selecting the implementation to the caller. Production can supply a real `FileReader`, and unit tests can supply a Moq object.

#### 4. When did the Moq-created object play a stub role, and when did it play a mock role?

It played a stub role when `.Setup(...).Returns(...)` supplied controlled file contents and the test asserted the calculator's result. It played a mock role when `.Verify(...)` checked that `Read("MagicNumbers.txt")` was called exactly once.

#### 5. Why should the unit and integration tests both be retained even though some input values overlap?

They provide different evidence. Unit tests isolate the calculator logic, while integration tests prove that the real reader and file system work with it. One layer can pass while a fault still exists in the other layer.

#### 6. In incomplete integration, how does a driver differ from a stub?

A driver replaces a missing caller and invokes the component under test. A stub replaces a missing called dependency and supplies responses to the component under test.

#### 7. What evidence should be inspected when a test passes locally but fails on Ubuntu CI?

Inspect the failing step, project, test, exception, stack trace, source line, and attempted path. Check for path separators, filename case, absolute paths, working-directory assumptions, missing committed files, line endings, permissions, output-copy settings, and environment-version differences.

## Quick self-check questions

### Why is `IFileReader` not a parent class?

It is an interface: a contract that implementing classes agree to follow. `FileReader` implements it; it does not inherit normal implementation code from it.

### Does `var fileReader = new FileReader()` cause tight coupling because of `var`?

No. `var` only asks the compiler to infer the local variable's type. Tight coupling occurs because the method constructs the concrete `FileReader` internally.

### Is `fileReader` an object created from the interface?

More precisely, it is a variable declared as `IFileReader` that refers to an object whose class implements `IFileReader`. The actual object may be a real `FileReader` or a Moq-generated implementation.

### What is the shortest definition of Dependency Injection?

Supply a dependency from outside the code that uses it instead of creating it inside.

### What is the simplest difference between a stub and a mock?

A stub controls **what comes back** from a dependency. A mock verifies **how the dependency was called**.

## Final mental model

```text
Production/integration path:
Caller → GenMagicNum → real FileReader → real file system

Isolated unit-test path:
Test → GenMagicNum → Moq IFileReader → controlled values

CI path:
Git push → GitHub Actions → restore → build → run all tests
```

Lab 3 therefore moves the solution from tests that only run locally to a layered, automatically checked design in which dependencies can be replaced deliberately according to the type of evidence each test needs.
