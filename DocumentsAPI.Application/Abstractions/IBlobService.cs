using DocumentsAPI.Application.Records;

namespace DocumentsAPI.Application.Abstractions;

public interface IBlobService
{
    Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<Guid> UploadPdfAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadPdfAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task DeletePdfAsync(Guid fileId, CancellationToken cancellationToken = default);
}
