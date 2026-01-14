using Microsoft.AspNetCore.Identity.UI.Services;

namespace Warframe_Utils_.NET.Services
{
    /// <summary>
    /// Development email sender that logs emails instead of sending them.
    /// 
    /// ASP.NET Identity requires an IEmailSender implementation.
    /// This implementation logs emails to the console instead of actually sending them.
    /// 
    /// For production, replace this with a real email service like:
    /// - SendGrid
    /// - AWS SES
    /// - SMTP service
    /// - Azure Communication Services
    /// </summary>
    public class DevEmailSender : IEmailSender
    {
        private readonly ILogger<DevEmailSender> _logger;

        public DevEmailSender(ILogger<DevEmailSender> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Simulates sending an email by logging it to the console.
        /// In production, this should actually send emails.
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("==============================================");
            _logger.LogInformation("EMAIL SIMULATION (Development Mode)");
            _logger.LogInformation("==============================================");
            _logger.LogInformation($"To: {email}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Message: {htmlMessage}");
            _logger.LogInformation("==============================================");
            
            // In development, we return a completed task
            // In production, replace this with actual email sending logic
            return Task.CompletedTask;
        }
    }
}
