using UserService.Models.Request;
using UserService.Validation;

namespace UserServiceTests.ValidationTests;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _createUserRequestValidator;

    public CreateUserRequestValidatorTests()
    {
        _createUserRequestValidator = new CreateUserRequestValidator();
    }

    [Fact]
    public async Task CreateUserRequestValidation_CorrectInput_Validates()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@gmail.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            Username = "UserOne",
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert

        Assert.True(result.IsValid);
        Assert.True(result.Errors.Count.Equals(0));
    }

    [Theory]
    [InlineData("This is a test")]
    [InlineData("a@b")]
    [InlineData("")]
    public async Task CreateUserRequestValidation_IncorrectEmailFormat_ValidationFailure(string email)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = email,
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            Username = "UserOne",
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "Email")
            ?.PropertyName ?? "";

        
        
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2 or 3);
        Assert.Equal("Email", propertyName);
    }
    
    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task CreateUserRequestValidation_IncorrectFirstName_ValidationFailure(string firstName)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@email.com",
            FirstName = firstName,
            LastName = "Doe",
            Password = "Password123",
            Username = "UserOne",
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "FirstName")
            ?.PropertyName ?? "";

        
        
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2);
        Assert.Equal("FirstName", propertyName);
    }
    
    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task CreateUserRequestValidation_IncorrectLastName_ValidationFailure(string lastName)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@gmail.com",
            FirstName = "John",
            LastName = lastName,
            Password = "Password123",
            Username = "UserOne",
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "LastName")
            ?.PropertyName ?? "";

        
        
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2);
        Assert.Equal("LastName", propertyName);
    }
    
    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task CreateUserRequestValidation_IncorrectPassword_ValidationFailure(string password)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@gmail.com",
            FirstName = "John",
            LastName = "Doe",
            Password = password,
            Username = "UserOne",
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "Password")
            ?.PropertyName ?? "";

        
        
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2);
        Assert.Equal("Password", propertyName);
    }
    
    [Theory]
    [InlineData("The quick brown fox jumps over the lazy dog while humming softly.")]
    [InlineData("A")]
    [InlineData("")]
    public async Task CreateUserRequestValidation_IncorrectUsername_ValidationFailure(string username)
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@gmail.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password123",
            Username = username,
        };
        // Act

        var result = await _createUserRequestValidator.ValidateAsync(request);
        // Assert
        string propertyName = result.Errors
            .FirstOrDefault(x => x.PropertyName == "Username")
            ?.PropertyName ?? "";

        
        
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count is 1 or 2 or 3);
        Assert.Equal("Username", propertyName);
    }
    
    
    
}