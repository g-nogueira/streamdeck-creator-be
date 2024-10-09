using StreamDeckBuddy.API.Endpoints;
using StreamDeckBuddy.Services;
using StreamDeckBuddy.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IIconService, StreamDeckIconPackIconService>();
builder.Services.AddSingleton<IUserIconCollectionService, FileSystemUserIconCollectionService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:")
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGroup("/user-icon-collections")
    .MapUserIconCollectionEndpoints();

app.MapGroup("/icons")
    .MapIconEndpoints();

app.MapDownloadEndpoints();

app.Run();