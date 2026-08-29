namespace Online_KinoTeater.UnitTests.Email;

public class EmailTests
{
    [Theory]
    [InlineData("test@mail.com")]
    [InlineData("test@mail.ru")]
    public void Create_Should_CreateEmail_When_EmailIsValid(string input)
    {
        // Act
        var email = Domain.Model.ValueObjects.Email.Create(input);
        
        // Assert
        Assert.True(email.IsSuccess);
        Assert.NotNull(email.Value);
        Assert.Equal(input, email.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("test.ru")]
    [InlineData("test")]
    [InlineData("test@mail")]
    [InlineData("test@@mail@")]
    [InlineData("test@mail.net")]
    public void Create_Should_ReturnFailure_When_EmailIsInvalid(string input)
    {
        // Act
        var email = Domain.Model.ValueObjects.Email.Create(input);
        
        // Assert
        Assert.False(email.IsSuccess);
    }
}