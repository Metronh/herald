using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Entities;
using UserService.Interfaces.Repository;
using UserService.Interfaces.Services;
using UserService.Messaging.Events;
using UserService.Models.Request;
using UserService.Services;
using UserService.Validation;

namespace UserServiceTests.ServiceTests;

public class AccountServiceTests
{
    private readonly Mock<ILogger<AccountService>> _logger;
    private readonly CreateUserRequestValidator _createUserRequestValidator;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly LoginRequestValidator _loginRequestValidator;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly AccountService _accountService;
    private readonly DeactivateAccountRequestValidator _deactivateAccountRequestValidator;

    public AccountServiceTests()
    {
        _logger = new Mock<ILogger<AccountService>>();
        _createUserRequestValidator = new CreateUserRequestValidator();
        _loginRequestValidator = new LoginRequestValidator();
        _userRepository = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher<UserEntity>>();
        _tokenService = new Mock<ITokenService>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _deactivateAccountRequestValidator = new DeactivateAccountRequestValidator();
        _accountService = new AccountService(
            logger: _logger.Object,
            createUserRequestValidator: _createUserRequestValidator,
            userRepository: _userRepository.Object,
            hasher: _passwordHasher.Object,
            loginRequestValidator: _loginRequestValidator,
            tokenService: _tokenService.Object,
            publishEndpoint: _publishEndpoint.Object,
            deactivateAccountRequestValidator: _deactivateAccountRequestValidator);
    }

    [Fact]
    public async Task CreateUser_ValidRequest_CreatesUserAndPublishesEvent()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "testPassword",
            Username = "Username123",
        };

        _passwordHasher.Setup(v =>
            v.HashPassword(It.IsAny<UserEntity>(), request.Password)).Returns("Hashed");

        // Act
        var result = await _accountService.CreateUser(request);
        // Assert
        Assert.True(result.IsAccountCreated);
        Assert.False(result.IsAdministratorAccount);
        _publishEndpoint.Verify(p => p.Publish(
            It.Is<SendWelcomeEmailEvent>(e => e.Email == request.Email),
            default), Times.Once);

        _passwordHasher.Verify(h => h.HashPassword(
            It.IsAny<UserEntity>(), request.Password), Times.Once);

        _userRepository.Verify(user => user.CreateUser(
            It.Is<UserEntity>(u => u.Email == request.Email)), Times.Once);
    }


    [Fact]
    public async Task CreateUser_InvalidRequest_CreatesUserAndNoPublishedEvent()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "this is a test",
            FirstName = "John",
            LastName = "Doe",
            Password = "testPassword",
            Username = "Username123",
        };

        _passwordHasher.Setup(v =>
            v.HashPassword(It.IsAny<UserEntity>(), request.Password)).Returns("Hashed");

        // Act
        var result = await _accountService.CreateUser(request);
        // Assert
        Assert.False(result.IsAccountCreated);
        Assert.True(result.ValidationFailures!.Count.Equals(1));
        Assert.False(result.IsAdministratorAccount);
    }

    [Fact]
    public async Task CreateUserAdmin_ValidRequest_CreatesUserAndPublishesEvent()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "testPassword",
            Username = "Username123",
        };

        _passwordHasher.Setup(v =>
            v.HashPassword(It.IsAny<UserEntity>(), request.Password)).Returns("Hashed");

        // Act
        var result = await _accountService.CreateUser(request, true);
        // Assert
        Assert.True(result.IsAccountCreated);
        Assert.True(result.IsAdministratorAccount);

        _publishEndpoint.Verify(p => p.Publish(
            It.Is<SendWelcomeEmailEvent>(e => e.Email == request.Email),
            default), Times.Once);

        _passwordHasher.Verify(h => h.HashPassword(
            It.IsAny<UserEntity>(), request.Password), Times.Once);

        _userRepository.Verify(user => user.CreateUser(
            It.Is<UserEntity>(u => u.Email == request.Email)), Times.Once);
    }

    [Fact]
    public async Task Login_ValidRequest_GetsToken()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username123",
            Password = "Secret",
        };
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Password = request.Password,
            Administrator = false,
            CreatedAt = DateTime.UtcNow,
            Email = "test@example.com",
            FailedLoginAttempts = 0,
            Id = Guid.NewGuid(),
            LockedOut = false,
            FirstName = "Jane",
            LastName = "Doe",
        };
        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Success);
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync(userEntity);
        _tokenService.Setup(t => t.GenerateToken(userEntity.Id, userEntity.Email, userEntity.Administrator))
            .Returns("Token");
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Token is not null && result.Token.Equals("Token"));
        Assert.False(result.AccountLocked);
        _userRepository.Verify(u =>
            u.RegisterLogin(It.Is<LoginSessionEntity>(uEntity => uEntity.UserId == userEntity.Id)), Times.Once);
        _userRepository.Verify(u => u.UpdateLoginAttempts(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task Login_InvalidRequest_Fails()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "u",
            Password = "Secret",
        };

        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Success);
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.Token is null);
    }

    [Fact]
    public async Task Login_UserDoesNotExist_Fails()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "user",
            Password = "Secret",
        };

        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Success);
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync((UserEntity?)null);

        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.Token is null);
    }

    [Fact]
    public async Task Login_WrongPassword_Increments()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username123",
            Password = "Secret",
        };
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Password = request.Password,
            Administrator = false,
            CreatedAt = DateTime.UtcNow,
            Email = "test@example.com",
            FailedLoginAttempts = 0,
            Id = Guid.NewGuid(),
            LockedOut = false,
            FirstName = "Jane",
            LastName = "Doe",
        };
        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Failed);
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync(userEntity);
        _tokenService.Setup(t => t.GenerateToken(userEntity.Id, userEntity.Email, userEntity.Administrator))
            .Returns("Token");
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.Token is null);
        Assert.False(result.AccountLocked);
        _userRepository.Verify(u =>
            u.UpdateLoginAttempts(It.Is<UserEntity>(uEntity => uEntity.FailedLoginAttempts == 1)), Times.Once);
    }

    [Fact]
    public async Task Login_WrongPasswordThresholdMet_LocksAccount()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username123",
            Password = "Secret",
        };
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Password = request.Password,
            Administrator = false,
            CreatedAt = DateTime.UtcNow,
            Email = "test@example.com",
            FailedLoginAttempts = 2,
            Id = Guid.NewGuid(),
            LockedOut = false,
            FirstName = "Jane",
            LastName = "Doe",
        };
        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Failed);
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync(userEntity);
        _tokenService.Setup(t => t.GenerateToken(userEntity.Id, userEntity.Email, userEntity.Administrator))
            .Returns("Token");
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.Token is null);
        Assert.True(result.AccountLocked);
        _userRepository.Verify(u =>
            u.UpdateLoginAttempts(It.Is<UserEntity>(uEntity => uEntity.FailedLoginAttempts == 3)), Times.Once);
        _userRepository.Verify(u => u.LockAccount(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task Login_CorrectPassword_ResetsFailedLoginAttempts()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username123",
            Password = "Secret",
        };
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Password = request.Password,
            Administrator = false,
            CreatedAt = DateTime.UtcNow,
            Email = "test@example.com",
            FailedLoginAttempts = 2,
            Id = Guid.NewGuid(),
            LockedOut = false,
            FirstName = "Jane",
            LastName = "Doe",
        };
        _passwordHasher.Setup(v =>
                v.VerifyHashedPassword(It.IsAny<UserEntity>(), It.IsAny<string>(), request.Password))
            .Returns(PasswordVerificationResult.Success);
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync(userEntity);
        _tokenService.Setup(t => t.GenerateToken(userEntity.Id, userEntity.Email, userEntity.Administrator))
            .Returns("Token");
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Token is not null && result.Token.Equals("Token"));
        Assert.False(result.AccountLocked);
        _userRepository.Verify(u =>
            u.UpdateLoginAttempts(It.Is<UserEntity>(uEntity => uEntity.FailedLoginAttempts == 0)), Times.Once);
        _userRepository.Verify(u => u.RegisterLogin(It.Is<LoginSessionEntity>(s => s.UserId == userEntity.Id)),
            Times.Once);
    }
    
    [Fact]
    public async Task Login_LockedOut_AccountLockedTrue()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "Username123",
            Password = "Secret",
        };
        var userEntity = new UserEntity
        {
            Username = request.Username,
            Password = request.Password,
            Administrator = false,
            CreatedAt = DateTime.UtcNow,
            Email = "test@example.com",
            FailedLoginAttempts = 3,
            Id = Guid.NewGuid(),
            LockedOut = true,
            FirstName = "Jane",
            LastName = "Doe",
        };
        _userRepository.Setup(u => u.GetUserLoginDetailsByUsername(request.Username)).ReturnsAsync(userEntity);
        // Act
        var result = await _accountService.Login(request);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.AccountLocked);
        
    }
}