using NUnit.Framework;
using SOFTEST_INTRO_Calculator;

namespace SOFTEST_INTRO_Calculator.IntegrationTests;

[TestFixture]
public sealed class MagicNumberFileIntegrationTests
{
    private Calculator _calculator = null!;
    private IFileReader _fileReader = null!;
    private string _path = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new Calculator();
        _fileReader = new FileReader();
        _path = Path.GetTempFileName();
        File.WriteAllLines(_path, new[] { "42", "-7" });
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }

    [TestCase(0, 84)]
    [TestCase(1, 14)]
    public void GenMagicNum_FileContainsNumber_ReturnsTwiceMagnitude(
        int choice, double expected)
    {
        double result = _calculator.GenMagicNum(choice, _path, _fileReader);

        Assert.That(result, Is.EqualTo(expected));
    }

    // Rejection Test
    [TestCase(-1)]
    [TestCase(2)]
    public void GenMagicNum_IndexOutsideFile_ThrowsArgumentOutOfRangeException(
        int choice)
    {
        Assert.That(
            () => _calculator.GenMagicNum(choice, _path, _fileReader),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}
