using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Line.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();