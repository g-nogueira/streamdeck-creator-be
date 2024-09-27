using StreamDeckBuddy.Models;
using StreamDeckBuddy.API.Endpoints;
using StreamDeckBuddy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IFileSystemIconService, FileSystemIconService>();
builder.Services.AddSingleton<ICollectionService, FileSystemCollectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIconEndpoints();
app.MapCollectionEndpoints();
app.MapDownloadEndpoints();

app.Run();