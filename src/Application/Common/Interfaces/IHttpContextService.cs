namespace Application.Common.Interfaces
{
    // Interface to Access Http Context Info
    public interface IHttpContextService
    {
        /// <summary>
        /// Gets the Origin Url of the Request
        /// </summary>
        /// <returns>The Orign Url</returns>
        string GetOrigin();
        /// <summary>
        /// Gets the Url Path of the Request
        /// </summary>
        /// <returns>The Url Path</returns>
        string GetPath();
        /// <summary>
        /// Gets the Ip Address of the Request Source
        /// </summary>
        /// <returns>The Ip Address</returns>
        string GetRequestIpAddress();
        /// <summary>
        /// Adds Custom Headers to the Http Response
        /// </summary>
        /// <param name="headerName">Name of the new Custom Header</param>
        /// <param name="value">Value for the new Custom Header</param>
        void AddHeaderValue(string headerName, object value);
    }
}
