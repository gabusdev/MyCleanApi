namespace Infrastructure.BackgroundJobs;

internal class HangfireSettings
{
    public string? Provider { get; set; }
    public string? Constring { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }
    internal static HangfireSettings GetHangfireSettings(IConfiguration config)
    {
        var settings = new HangfireSettings();

        settings.Provider = config.GetSection("HangfireSettings:Provider").Value;
        settings.Constring = config.GetSection("HangfireSettings:Constring").Value;
        settings.User = config.GetSection("HangfireSettings:Credentials:User").Value;
        settings.Password = config.GetSection("HangfireSettings:Credentials:Password").Value;

        return settings;
    }
}



