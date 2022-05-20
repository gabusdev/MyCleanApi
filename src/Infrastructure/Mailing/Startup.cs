using Application.Common.Mailing;

namespace Infrastructure.Mailing;

internal static class Startup
{
    internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration config) =>
        services
            //.Configure<MailSettings>(config.GetSection(nameof(MailSettings)))
            .AddTransient<IEmailTemplateService, EmailTemplateService>()
            .AddTransient<IMailService, SmtpMailService>();
}