namespace Infrastructure.Mailing;

public class MailSettings
{
    public string? From { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? DisplayName { get; set; }

    internal static MailSettings GetMailSettings(IConfiguration config)
    {
        var mailSettings = config.GetSection(nameof(MailSettings)).Get<MailSettings>();

        GetSettingsFromEnv(mailSettings);

        if (mailSettings.UserName == null || mailSettings.Password == null)
        {
            throw new InvalidOperationException("There are not username or password provided for mailing");
        }

        return mailSettings;
    }

    private static void GetSettingsFromEnv(MailSettings mailSettings)
    {
        var userName = Environment.GetEnvironmentVariable("mailUsername", EnvironmentVariableTarget.User);
        if (userName != null)
        {
            mailSettings.UserName = userName;
        }

        var password = Environment.GetEnvironmentVariable("mailPass", EnvironmentVariableTarget.User);
        if (password != null)
        {
            mailSettings.Password = password;
        }
    }
}