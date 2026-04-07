using MassTransit;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Interfaces.Repository;
using UserService.Interfaces.Services;
using UserService.Messaging.Events;
using UserService.Models.Request;
using UserService.Models.Response;
using UserService.Validation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace UserService.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly CreateUserRequestValidator _createUserRequestValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserEntity> _hasher;
    private readonly LoginRequestValidator _loginRequestValidator;
    private readonly DeactivateAccountRequestValidator _deactivateAccountRequestValidator;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;

    public AccountService(ILogger<AccountService> logger, CreateUserRequestValidator createUserRequestValidator,
        IUserRepository userRepository, IPasswordHasher<UserEntity> hasher, LoginRequestValidator loginRequestValidator,
        ITokenService tokenService, IPublishEndpoint publishEndpoint, DeactivateAccountRequestValidator deactivateAccountRequestValidator)
    {
        _logger = logger;
        _createUserRequestValidator = createUserRequestValidator;
        _userRepository = userRepository;
        _hasher = hasher;
        _loginRequestValidator = loginRequestValidator;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
        _deactivateAccountRequestValidator = deactivateAccountRequestValidator;
    }

    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request, bool isAdministrator = false)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(AccountService), nameof(CreateUser), DateTime.UtcNow);
        var response = new CreateUserResponse
        {
            IsAccountCreated = false,
            IsAdministratorAccount = isAdministrator,
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
            CreatedAt = DateTime.UtcNow,
        };

        user.Password = _hasher.HashPassword(user, request.Password);
        await _userRepository.CreateUser(user: user);
        response.IsAccountCreated = true;

        await _publishEndpoint.Publish(new SendWelcomeEmailEvent
        {
            Email = user.Email,
            FirstName = user.FirstName,
            UserId = user.Id
        });

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

        var user = await _userRepository.GetUserLoginDetailsByUsername(request.Username);

        if (user is null)
            return response;

        if (user.LockedOut)
        {
            response.AccountLocked = user.LockedOut;
            return response;
        }

        var isCorrectPassword = _hasher.VerifyHashedPassword(user: user, user.Password!, request.Password);

        if (isCorrectPassword == PasswordVerificationResult.Failed)
        {
            user.FailedLoginAttempts++;
            await _userRepository.UpdateLoginAttempts(user);
            if (user.FailedLoginAttempts == 3)
            {
                await _userRepository.LockAccount(user);
                response.AccountLocked = true;
            }

            return response;
        }

        response.Token = _tokenService.GenerateToken(user.Id, user.Email, user.Administrator);

        var loginSession = new LoginSessionEntity
        {
            LoginSessionId = Guid.NewGuid(),
            UserId = user.Id,
            LoginTime = DateTime.UtcNow,
            LogoutTime = _tokenService.GetSessionExpiryTime(),
        };

        await _userRepository.RegisterLogin(loginSession);
        if (user.FailedLoginAttempts != 0)
        {
            user.FailedLoginAttempts = 0;
            await _userRepository.UpdateLoginAttempts(user);
        }
        response.Success = true;

        _logger.LogInformation("{Class}.{Method} completed at {Time}",
            nameof(AccountService), nameof(Login), DateTime.UtcNow);
        return response;
    }

    public async Task<DeactivateAccountResponse> DeactivateAccount(DeactivateAccountRequest request)
    {
        _logger.LogInformation("{Class}.{Method} started at {Time}",
            nameof(AccountService), nameof(DeactivateAccount), DateTime.UtcNow);
        var response = new DeactivateAccountResponse
        {
            Success = false,
            ValidationFailures = new List<ValidationFailureResponse>()
        };

        var validationResult = await _deactivateAccountRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            response.ValidationFailures = validationResult.Errors.Select(e => new ValidationFailureResponse
            {
                Property = e.PropertyName,
                ErrorMessage = e.ErrorMessage,
            }).ToList();
            return response;
        }

        var user = await _userRepository.GetUserLoginDetailsByUsername(request.Username);
        
        if (user is null)
            return response;

        var isCorrectPassword = _hasher.VerifyHashedPassword(user: user, user.Password!, request.Password);

        if (isCorrectPassword == PasswordVerificationResult.Failed)
            return response;

        await _userRepository.DeactivateAccount(user);

        response.Success = true;
        return response;
    }
}