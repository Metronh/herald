using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Interfaces.Services;
using UserService.Models.Request;
using UserService.Models.Response;

namespace UserService.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        app.MapPost("/CreateUser", CreateUser);
        app.MapPost("/CreateUser/Administrator", CreateAdministratorUser);
        app.MapPost("/Login", Login);
        app.MapPatch("/DeactivateAccount", DeactivateAccount).RequireAuthorization();
    }

    private static async Task<Results<Ok<CreateUserResponse>, ProblemHttpResult>> CreateUser(CreateUserRequest request,
        ILogger<Program> logger, IAccountService accountService)
    {
        logger.LogInformation("/CreateUser endpoint hit at {utcTime}", DateTime.UtcNow);
        var response = await accountService.CreateUser(request);
        logger.LogInformation("/CreateUser endpoint Completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok(response);
    }
    private static async Task<Results<Ok<CreateUserResponse>, ProblemHttpResult>> CreateAdministratorUser(CreateUserRequest request,
        ILogger<Program> logger, IAccountService accountService)
    {
        logger.LogInformation("/CreateUser/Administrator endpoint hit at {utcTime}", DateTime.UtcNow);
        var response = await accountService.CreateUser(request: request, isAdministrator: true);
        logger.LogInformation("/CreateUser/Administrator endpoint Completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<LoginResponse>, ProblemHttpResult>> Login(LoginRequest request,
        HttpContext httpContext, ILogger<Program> logger, IAccountService accountService)
    {
        logger.LogInformation("/Login endpoint hit at {utcTime}", DateTime.UtcNow);

        var loginResponse = await accountService.Login(request);
        
        logger.LogInformation("/Login endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok(loginResponse);
    }
    
    private static async Task<Results<Ok<DeactivateAccountResponse>, ProblemHttpResult>> DeactivateAccount(DeactivateAccountRequest request,
        HttpContext httpContext, ILogger<Program> logger, IAccountService accountService)
    {
        logger.LogInformation("/DeactivateAccount endpoint hit at {utcTime}", DateTime.UtcNow);

        var deactivateAccountResponse = await accountService.DeactivateAccount(request);
        
        logger.LogInformation("/DeactivateAccount endpoint completed at {utcTime}", DateTime.UtcNow);
        return TypedResults.Ok(deactivateAccountResponse);
    }
}