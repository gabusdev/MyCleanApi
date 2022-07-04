using Application.Common.FileStorage;
using Domain.Common;
using Infrastructure.Common.Extensions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    public async Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class
    {
        // Max Size in bytes
        int maxZise = 5_000_000;


        if (request == null || request.Data == null)
        {
            return string.Empty;
        }

        if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()))
        {
            throw new InvalidOperationException("File Format Not Supported.");
        }

        if (request.Name is null)
        {
            throw new InvalidOperationException("Name is required.");
        }

        var fileData = Regex.Match(request.Data, "data:(?<type>.+?)/(?<extension>.+?);base64,(?<data>.+)");
        string type = fileData.Groups["type"].Value;
        string extension = fileData.Groups["extension"].Value;
        string base64Data = fileData.Groups["data"].Value;

        // Check if Base64 String Type is supported
        if (!CheckType(type))
        {
            throw new InvalidOperationException("The File Type provided is not supported.");
        }

        // Check if Base64 String Extension == File Extension provided
        if (!CheckExtension(extension, request.Extension))
        {
            throw new InvalidOperationException("The File Extensions provided do not match.");
        }

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));

        // Check for max File Size
        if (streamData.Length > maxZise)
        {
            throw new InvalidOperationException("File Size is too large");
        }

        if (streamData.Length > 0)
        {
            string folder = typeof(T).Name;
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                folder = folder.Replace(@"\", "/");
            }

            string folderName = supportedFileType switch
            {
                FileType.Image => Path.Combine("Files", "Images", folder),
                _ => Path.Combine("Files", "Others", folder),
            };
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(pathToSave);

            string fileName = GenerateRandomFileName(request.Extension.Trim());
            string fullPath = Path.Combine(pathToSave, fileName);
            string dbPath = Path.Combine(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            using var stream = new FileStream(fullPath, FileMode.Create);
            await streamData.CopyToAsync(stream, cancellationToken);
            return dbPath.Replace("\\", "/");
        }
        else
        {
            return string.Empty;
        }
    }

    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    public void Remove(string? path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        if (Path.HasExtension(path))
        {
            return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
        }

        return GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1, max = 2;

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(pattern, max);
    }

    private bool CheckType(string base64Type)
    {
        foreach (var item in Enum.GetNames(typeof(FileType)))
        {
            if (item.ToLower() == base64Type.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckExtension(string base64Extension, string extension)
    {
        return base64Extension.ToLower() == extension[1..].ToLower();
    }

    private string GenerateRandomFileName(string? extension = null)
    {
        var filename = Path.GetRandomFileName();
        filename += extension is null ? string.Empty : $"{extension}";
        return filename;
    }
}