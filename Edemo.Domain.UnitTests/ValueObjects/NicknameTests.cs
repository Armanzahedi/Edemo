using Edemo.Domain.TopUp.ValueObjects;

namespace Edemo.Domain.UnitTests.ValueObjects;

public class NicknameTests
{
    [Fact]
    public void Create_WithValidNickname_ShouldSucceed()
    {
        // Arrange
        var validNickname = "ValidNickname";

        // Act
        var nickname = Nickname.Create(validNickname);

        // Assert
        nickname.Value.Should().Be(validNickname);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrEmpty_ShouldThrowArgumentException(string invalidNickname)
    {
        // Act
        Action act = () => Nickname.Create(invalidNickname);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nickname cannot be empty.");
    }

    [Fact]
    public void Create_WithTooLongNickname_ShouldThrowArgumentException()
    {
        // Arrange
        var longNickname = new string('a', 21); // 21 characters long

        // Act
        Action act = () => Nickname.Create(longNickname);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nickname must be 20 characters or fewer.");
    }
}