using Domain.Common;

namespace Application.Common.FileStorage;

public interface IFileStorageService
{
    /// <summary>
    /// Attemps to Store the Base64 File from the <c>FileUploadRequest</c>
    /// </summary>
    /// <typeparam name="T">Object Type that determines the folder where the uploaded file resides</typeparam>
    /// <param name="request">The <c>FileUploadRequest</c> object to be saved</param>
    /// <param name="supportedFileType">The file type of the object for checking purpouse</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The route where the file can be accesed</returns>
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;
    /// <summary>
    /// Attemps to remove a file
    /// </summary>
    /// <param name="path">Path where the file resides</param>
    public void Remove(string? path);
}