using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentsAPI.Application.Abstractions;
using DocumentsAPI.Application.Records;

namespace DocumentsAPI.Infrastructure.Storage;

internal class BlobService(BlobServiceClient blobServiceClient) : IBlobService
{
    private const string ContainerName = "documents";
    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        var response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        var fileId = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken);

        return fileId;
    }
}
