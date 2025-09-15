using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text.RegularExpressions;

public class CloudinaryStorage : ICloudStorage
{
    private readonly Cloudinary _cloudinary;
    private readonly CloudinarySettings _settings;

    public CloudinaryStorage(Cloudinary cloudinary, IOptions<CloudinarySettings> settings)
    {
        _cloudinary = cloudinary;
        _settings = settings.Value;
    }

    public async Task<string> UploadAsync(
        IFormFile file,
        string fileNameHint = null,
        string folderId = null,
        CancellationToken ct = default)
    {
        if (file == null || file.Length == 0) throw new ArgumentException("Arquivo vazio.");
        var folder = string.IsNullOrWhiteSpace(folderId) ? _settings.Folder : folderId;

        var safeBase = MakeSafeFileName(string.IsNullOrWhiteSpace(fileNameHint) ? file.FileName : fileNameHint);
        var publicId = $"{safeBase}_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}";

        var contentType = file.ContentType?.ToLowerInvariant() ?? "application/octet-stream";
        var isImage = contentType.StartsWith("image/");

        using var stream = file.OpenReadStream();

        if (isImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Folder = string.IsNullOrWhiteSpace(folder) ? null : folder,
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = false
            };

            ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams, ct);

            if (result.StatusCode is not System.Net.HttpStatusCode.OK)
                throw new Exception($"Falha upload Cloudinary (imagem): {result.Error?.Message}");

            return result.SecureUrl?.AbsoluteUri ?? result.Url?.AbsoluteUri;
        }
        else
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Folder = string.IsNullOrWhiteSpace(folder) ? null : folder,
                UseFilename = false,
                UniqueFilename = false,
                Overwrite = false
            };

            RawUploadResult result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode is not System.Net.HttpStatusCode.OK)
                throw new Exception($"Falha upload Cloudinary (raw): {result.Error?.Message}");

            return result.SecureUrl?.AbsoluteUri ?? result.Url?.AbsoluteUri;
        }
    }
    private static string MakeSafeFileName(string name)
    {
        var baseName = Path.GetFileNameWithoutExtension(name);
        var ext = Path.GetExtension(name);
        baseName = baseName.ToLowerInvariant();
        baseName = Regex.Replace(baseName, @"\s+", "-");
        baseName = Regex.Replace(baseName, @"[^a-z0-9\-_]", "");
        if (string.IsNullOrWhiteSpace(baseName)) baseName = "arquivo";
        return baseName + ext.ToLowerInvariant();
    }
}
