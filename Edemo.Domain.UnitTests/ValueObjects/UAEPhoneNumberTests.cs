using Edemo.Domain.TopUp.ValueObjects;

namespace Edemo.Domain.UnitTests.ValueObjects;

public class UAEPhoneNumberTests
{
    [Theory]
    [InlineData("0501234567")]
    [InlineData("+971501234567")]
    [InlineData("971501234567")]
    public void Create_WithValidUaePhoneNumber_ShouldSucceed(string validNumber)
    {
        // Act
        var action = () => UAEPhoneNumber.Create(validNumber);

        // Assert
        action.Should().NotThrow();
        var phoneNumber = action.Invoke();
        phoneNumber.Number.Should().Be(validNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyString_ShouldThrowArgumentException(string invalidNumber)
    {
        // Act
        Action action = () => UAEPhoneNumber.Create(invalidNumber);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Phone number cannot be empty.");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("+123456789012")]
    public void Create_WithInvalidUaePhoneNumberFormat_ShouldThrowArgumentException(string invalidNumber)
    {
        // Act
        Action action = () => UAEPhoneNumber.Create(invalidNumber);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Phone number is not in a valid UAE format.");
    }
}