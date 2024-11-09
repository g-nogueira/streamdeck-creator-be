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
            .AllowAnyOrigin();
        // TODO: Improve CORS handling
        // .WithOrigins("http://localhost:")
        // .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "127.0.0.1");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context)));

app.Map("/error",
    () => { throw new InvalidOperationException("An Error Occurred... Check the logs for more information."); });

app.MapGroup("/user-icon-collections")
    .MapUserIconCollectionEndpoints();

app.MapGroup("/icons")
    .MapIconEndpoints();

app.MapDownloadEndpoints();

app.Run();