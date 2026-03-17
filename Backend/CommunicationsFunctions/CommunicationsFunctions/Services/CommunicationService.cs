using System.ComponentModel.DataAnnotations;
using CommunicationsFunctions.Constants;
using CommunicationsFunctions.Interfaces.Repository;
using CommunicationsFunctions.Interfaces.Services;
using CommunicationsFunctions.Models;
using CommunicationsFunctions.Models.Entities;
using CommunicationsFunctions.Models.Enums;
using CommunicationsFunctions.Validators;
using Microsoft.Extensions.Logging;

namespace CommunicationsFunctions.Services;

public class CommunicationService : ICommunicationService
{
    private readonly ILogger<CommunicationService> _logger;
    private readonly IEmailService _emailService;
    private readonly SendEmailRequestValidator _emailRequestValidator;
    private readonly IUsersRepository _usersRepository;

    public CommunicationService(ILogger<CommunicationService> logger, IEmailService emailService,
        SendEmailRequestValidator emailRequestValidator, IUsersRepository usersRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _emailRequestValidator = emailRequestValidator;
        _usersRepository = usersRepository;
    }

    public async Task SendWelcomeEmailCommunication(SendWelcomeEmailRequest request)
    {
        var validationResult = await _emailRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException();
        var sendEmailRequest = new SendEmailRequest
        {
            Email = request.Email,
            FirstName = request.FirstName,
            Subject = EmailTitles.Welcome,
            TemplateName = EmailTemplateNames.WelcomeEmailTemplateName,
        };
        
        await _usersRepository.RegisterCommunication(new CommunicationEntity
        {
            CommunicationId = request.CommunicationId,
            CommunicationAddress = request.Email,
            CommunicationTitle = EmailTitles.Welcome,
            CommunicationType = CommunicationType.Email,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            Status = nameof(CommunicationStatus.Pending),
        });
        
        await _emailService.SendEmail(sendEmailRequest);

        await _usersRepository.SuccessfulCommunications(request.CommunicationId);
    }
}