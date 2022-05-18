namespace Infrastructure.OpenApi
{
    internal class SwaggerSettings
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Contact_Name { get; set; } = null!;
        public string Contact_Url { get; set; } = null!;
        public bool Auth { get; set; } = true;
    }
}
