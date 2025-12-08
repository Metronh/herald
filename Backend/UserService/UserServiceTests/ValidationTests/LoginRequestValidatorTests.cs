using UserService.Models.Request;
using UserService.Validation;

namespace UserServiceTests.ValidationTests;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _loginRequestValidator;

    public LoginRequestValidatorTests()
    {
        _loginRequestValidator = new LoginRequestValidator();
    }

    [Fact]
    public async Task LoginRequestValidator_CorrectInput_Validates()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username1",
            Password = "Password123",
        };

        // Act
        var result = await _loginRequestValidator.ValidateAsync(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task LoginRequestValidator_IncorrectUsername_Fails(string username)
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = username,
            Password = "Password123"
        };
        // Act
        var result = await _loginRequestValidator.ValidateAsync(request);

        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "Username")
            ?.PropertyName ?? "";


        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2 or 3);
        Assert.Equal("Username", propertyName);
    }

    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task LoginRequestValidator_IncorrectPassword_Fails(string password)
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username1",
            Password = password
        };
        // Act
        var result = await _loginRequestValidator.ValidateAsync(request);

        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "Password")
            ?.PropertyName ?? "";


        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2);
        Assert.Equal("Password", propertyName);
    }
}