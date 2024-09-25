using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Line.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

// 添加Line Messaging Client
builder.Services.AddHttpClient<ILineMessagingClient, LineMessagingClient>(httpClient =>
{
    var channelAccessToken = builder.Configuration["LineSettings:ChannelAccessToken"];
    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {channelAccessToken}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (errorFeature != null)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError($"Unhandled exception: {errorFeature.Error}");
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        });
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application starting...");

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";
logger.LogInformation($"Starting application on {url}");
app.Run(url);