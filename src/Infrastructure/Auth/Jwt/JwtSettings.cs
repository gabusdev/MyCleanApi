namespace Infrastructure.Auth.Jwt
{
    internal class JwtSettings
    {
        public string Key { get; set; } = null!;

        public string ValidIssuer { get; set; } = null!;
        public string ValidAudience { get; set; } = null!;
        public int ExpirationInMinutes { get; set; }
        public int RefreshExpirationInDays { get; set; }

        public bool Encrypt { get; set; }
        public string Secret { get; set; } = null!;

        internal static JwtSettings GetJwtSettings(IConfiguration config)
        {
            var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            GetSettingsFromEnv(jwtSettings);
            if (jwtSettings.Key is null || (jwtSettings.Encrypt is true && jwtSettings.Secret is null))
            {
                throw new InvalidOperationException("There are not Keys provided for Jwt");
            }

            if (jwtSettings.ExpirationInMinutes == default)
            {
                jwtSettings.ExpirationInMinutes = 15;
            }

            if (jwtSettings.RefreshExpirationInDays == default)
            {
                jwtSettings.RefreshExpirationInDays = 1;
            }

            return jwtSettings;
        }

        private static void GetSettingsFromEnv(JwtSettings jwtSettings)
        {
            var jwtKey = Environment.GetEnvironmentVariable("jwtKey", EnvironmentVariableTarget.User);
            if (jwtKey != null)
            {
                jwtSettings.Key = jwtKey;
            }

            var jwtSecret = Environment.GetEnvironmentVariable("jwtSecret", EnvironmentVariableTarget.User);
            if (jwtSecret != null)
            {
                jwtSettings.Secret = jwtSecret;
            }
        }
    }
}
