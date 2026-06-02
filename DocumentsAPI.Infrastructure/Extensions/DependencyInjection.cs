using Azure.Storage.Blobs;
using DocumentsAPI.Application.Abstractions;
using DocumentsAPI.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentsAPI.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBlobService, BlobService>();
        services.AddSingleton(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));
        return services;
    }
}
