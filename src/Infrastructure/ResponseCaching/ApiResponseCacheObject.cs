using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.ResponseCaching
{
    public class ApiResponseCacheObject
    {
        public ObjectResult Result { get; set; } = null!;
        public Dictionary<string, string> Headers { get; set; } = new();
        public string? Authorization { get; set; }
    }
}
