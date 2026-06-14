using DocumentsAPI.Application.Abstractions;
using InnoClinic.Contracts.Events.Appointments;
using MassTransit;

namespace DocumentsAPI.Infrastructure.Consumers;

public sealed class SaveAppointmentResultDocumentConsumer : IConsumer<ISaveAppointmentResultDocumentEvent>
{
    private readonly IBlobService _blobService;

    public SaveAppointmentResultDocumentConsumer(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task Consume(ConsumeContext<ISaveAppointmentResultDocumentEvent> context)
    {
        var message = context.Message;

        if (message.PdfBytes == null || message.PdfBytes.Length == 0)
            return;

        using var stream = new MemoryStream(message.PdfBytes);

        Guid fileId = await _blobService.UploadAsync(stream, message.ContentType, context.CancellationToken);
    }
}