using Line.Messaging;
using Line.Messaging.Webhooks;

public class LineBotApp : WebhookApplication
{
    private readonly ILineMessagingClient _messagingClient;
    private readonly string _channelSecret;

    public LineBotApp(ILineMessagingClient lineMessagingClient, string channelSecret)
    {
        _messagingClient = lineMessagingClient;
        _channelSecret = channelSecret;
    }

    protected override async Task OnMessageAsync(MessageEvent ev)
    {
        var result = await _messagingClient.ReplyMessageAsync(ev.ReplyToken, "您好！我收到了您的消息。");
    }
}