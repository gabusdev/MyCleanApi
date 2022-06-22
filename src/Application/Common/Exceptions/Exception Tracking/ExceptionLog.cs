using Domain.Common.Contracts;

namespace Application.Common.Exceptions.Exception_Tracking;
public class ExceptionLog : IEntity<string>
{
    public string Id { get; set; } = null!;
    public string? UserId { get; set; }
    public DateTime DataTime { get; set; }
    public string? Reference { get; set; }
    public string? IPInfo { get; set; }
    public string? ErrorDescription { get; set; }
    public string? Data { get; set; }
    public string? StackTrace { get; set; }
    public string? Source { get; set; }
    public string? Messages { get; set; }
}
