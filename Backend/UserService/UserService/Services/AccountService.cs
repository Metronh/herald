using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Interfaces.Repository;
using UserService.Interfaces.Services;
using UserService.Models.Request;
using UserService.Models.Response;
using UserService.Validation;

namespace UserService.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly CreateUserRequestValidator _createUserRequestValidator;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<UserEntity> _hasher;
    private readonly LoginRequestValidator _loginRequestValidator;
    private readonly ITokenService _tokenService;

    public AccountService(ILogger<AccountService> logger, CreateUserRequestValidator createUserRequestValidator,
        IUserRepository userRepository, PasswordHasher<UserEntity> hasher, LoginRequestValidator loginRequestValidator,
        ITokenService tokenService)
    {
        _logger = logger;
        _createUserRequestValidator = createUserRequestValidator;
        _userRepository = userRepository;
        _hasher = hasher;
        _loginRequestValidator = loginRequestValidator;
        _tokenService = tokenService;
    }

    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request, bool isAdministrator = false)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(AccountService), nameof(CreateUser), DateTime.UtcNow);
        var response = new CreateUserResponse
        {
            IsAccountCreated = false,
        };
        ValidationResult validationResult = await _createUserRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Account {request} not created because account info not valid",
                request.Username);
            response.ValidationFailures = validationResult.Errors.Select(e => new ValidationFailureResponse
            {
                Property = e.PropertyName,
                ErrorMessage = e.ErrorMessage,
            }).ToList();
            return response;
        }

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Administrator = isAdministrator,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
        };

        user.Password = _hasher.HashPassword(user, request.Password);
        await _userRepository.CreateUser(user: user);
        response.IsAccountCreated = true;
        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(AccountService), nameof(CreateUser), DateTime.UtcNow);
        return response;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(AccountService), nameof(Login), DateTime.UtcNow);

        var response = new LoginResponse
        {
            Success = false,
            ValidationFailures = new List<ValidationFailureResponse>(),
        };

        ValidationResult validationResult = await _loginRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Account {request} not logged in because account info not valid",
                request.Username);
            response.ValidationFailures = validationResult.Errors.Select(e => new ValidationFailureResponse
            {
                Property = e.PropertyName,
                ErrorMessage = e.ErrorMessage,
            }).ToList();

            return response;
        }

        var user = await _userRepository.GetUserLoginDetails(request.Username);

        if (user is null)
            return response;
        

        var isCorrectPassword = _hasher.VerifyHashedPassword(user: user, user.Password, request.Password);

        if (isCorrectPassword == PasswordVerificationResult.Failed)
            return response;

        response.Token = _tokenService.GenerateToken(user.Id, user.Email, user.Administrator);
        response.Success = true;

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(AccountService), nameof(Login), DateTime.UtcNow);
        return response;
    }
}