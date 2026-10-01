using APICourse.Extentions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//ConfigureServices
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureMapping();
builder.Services.AddSwaggerLock();
builder.Services.JwtAuthenticationService(builder.Configuration);
builder.Services.ConfigureService();
builder.Services.ConfigureRepository();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
