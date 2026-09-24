using Moq;
using NUnit.Framework;
using SOFTEST_INTRO_Calculator;

namespace SOFTEST_INTRO_Calculator.UnitTests;

[TestFixture]
public sealed class MagicNumberUnitTests
{
    private Calculator _calculator = null!;
    private Mock<IFileReader> _fileReader = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new Calculator();
        _fileReader = new Mock<IFileReader>();

        _fileReader
            .Setup(reader => reader.Read("MagicNumbers.txt"))
            .Returns(new[] { "42", "-7" });
    }

    [TestCase(0, 84)]
    [TestCase(1, 14)]
    public void GenMagicNum_ConfiguredValues_ReturnsTwiceMagnitude(
        int choice, double expected)
    {
        double result = _calculator.GenMagicNum(
            choice, "MagicNumbers.txt", _fileReader.Object);

        Assert.That(result, Is.EqualTo(expected));
    }

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

    [Test]
    public void GenMagicNum_ValidChoice_ReadsSuppliedPathOnce()
    {
        _calculator.GenMagicNum(
            0, "MagicNumbers.txt", _fileReader.Object);

        _fileReader.Verify(
            reader => reader.Read("MagicNumbers.txt"),
            Times.Once());
    }
}
