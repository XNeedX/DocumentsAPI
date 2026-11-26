using DocumentsAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("BlobStorage:Initialize"))
{
    await app.Services.InitializeBlobStorageAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
