using DocumentsAPI.Application.Abstractions;
using DocumentsAPI.Application.Errors;
using Microsoft.AspNetCore.Mvc;
using Profiles.Presentation.Controllers;
using Services.Application.Results;

namespace DocumentsAPI.Presentation.Controllers;

[Route("api/[controller]")]
public class DocumentsController : ApiController
{
    private readonly IBlobService _blobService;

    public DocumentsController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return HandleFailure(Result.Failure(FileErrors.Empty));

        using var stream = file.OpenReadStream();
        var fileId = await _blobService.UploadAsync(stream, file.ContentType, cancellationToken);

        return HandleResult(Result<Guid>.Success(fileId), "Файл успешно загружен.");
    }

    [HttpPost("upload-pdf")]
    public async Task<IActionResult> UploadPdf(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return HandleFailure(Result.Failure(FileErrors.Empty));

        using var stream = file.OpenReadStream();
        var fileId = await _blobService.UploadPdfAsync(stream, file.ContentType, cancellationToken);

        return HandleResult(Result<Guid>.Success(fileId), "PDF файл успешно загружен.");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _blobService.DownloadAsync(id, cancellationToken);
            return File(response.Stream, response.ContentType);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return HandleFailure(Result.Failure(FileErrors.NotFound));
        }
        catch (Exception)
        {
            return HandleFailure(Result.Failure(FileErrors.DownloadFailed));
        }
    }
    [HttpGet("{id:guid}/download-pdf")]
    public async Task<IActionResult> DownloadPdf(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _blobService.DownloadPdfAsync(id, cancellationToken);
            return File(response.Stream, response.ContentType);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return HandleFailure(Result.Failure(FileErrors.NotFound));
        }
        catch (Exception)
        {
            return HandleFailure(Result.Failure(FileErrors.DownloadFailed));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _blobService.DeleteAsync(id, cancellationToken);
            return HandleResult(Result.Success(), "File succesfully deleted.");
        }
        catch (Exception)
        {
            return HandleFailure(Result.Failure(FileErrors.DeleteFailed));
        }
    }
    [HttpDelete("{id:guid}/delete-pdf")]
    public async Task<IActionResult> DeletePdf(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _blobService.DeletePdfAsync(id, cancellationToken);
            return HandleResult(Result.Success(), "PDF файл успешно удален.");
        }
        catch (Exception)
        {
            return HandleFailure(Result.Failure(FileErrors.DeleteFailed));
        }
    }
}