using StreamDeckBuddy.Models;
using StreamDeckBuddy.API.Endpoints;
using StreamDeckBuddy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var icons = new List<Icon>();
var collections = new List<Collection>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIconEndpoints(icons);
app.MapCollectionEndpoints(collections);
app.MapDownloadEndpoints(icons, collections);

app.Run();