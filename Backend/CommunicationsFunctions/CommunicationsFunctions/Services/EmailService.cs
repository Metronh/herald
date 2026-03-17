using System.Reflection;
using CommunicationsFunctions.AppSettings;
using CommunicationsFunctions.Interfaces.Services;
using CommunicationsFunctions.Models;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Resend;

namespace CommunicationsFunctions.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IResend _resend;
    private readonly ResendSettings _resendSettings;
    private readonly ResiliencePipelineProvider<string> _pipelineBuilder;
    private const string EmailTemplatesFolder = "EmailTemplates";
    private const string ResendPipelineName = "ResendPipeline";

    public EmailService(ILogger<EmailService> logger,
        IResend resend, ResendSettings resendSettings, ResiliencePipelineProvider<string> pipelineBuilder)
    {
        _logger = logger;
        _resend = resend;
        _resendSettings = resendSettings;
        _pipelineBuilder = pipelineBuilder;
    }

    public async Task SendEmail(SendEmailRequest request)
    {
        string emailTemplate = await GetLocalHtmlTemplate(request.TemplateName);
        var message = new EmailMessage
        {
            From = _resendSettings.FromAddress,
            To = [request.Email],
            Subject = request.Subject,
            HtmlBody = emailTemplate,
        };
        var pipeline = _pipelineBuilder.GetPipeline(ResendPipelineName);
        _logger.LogInformation("Sending email to {ToEmailAddress}", request.Email);
        await pipeline.ExecuteAsync(async ct => await _resend.EmailSendAsync(message, ct));
        _logger.LogInformation("Email sent successfully to {ToEmailAddress}", request.Email);
    }

    private async Task<string> GetLocalHtmlTemplate(string templateName)
    {
        string templateFileName = $"{templateName}.html";
        var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            EmailTemplatesFolder, templateFileName);
        return await File.ReadAllTextAsync(templatePath);
    }
}