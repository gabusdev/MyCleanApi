namespace Application.Common.Interfaces
{
    public interface IHttpContextService
    {
        string GetOrigin();
        string GetPath();
        string GetRequestIpAddress();
        void AddHeaderValue(string headerName, object value);
    }
}
