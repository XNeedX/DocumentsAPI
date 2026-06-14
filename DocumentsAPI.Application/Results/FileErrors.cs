using Services.Application.Results;

namespace DocumentsAPI.Application.Errors;

public static class FileErrors
{
    public static readonly Error Empty = new(
        "File.Empty",
        "File was empty.",
        ErrorType.Validation);

    public static readonly Error NotFound = new(
        "File.NotFound",
        "File not found.",
        ErrorType.NotFound);

    public static readonly Error DownloadFailed = new(
        "File.DownloadFailed",
        "Error occurred while downloading the file.",
        ErrorType.Failure);

    public static readonly Error DeleteFailed = new(
        "File.DeleteFailed",
        "Error occurred while deleting the file.",
        ErrorType.Failure);
}