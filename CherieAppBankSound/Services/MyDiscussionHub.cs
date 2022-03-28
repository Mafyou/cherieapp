namespace CherieAppBankSound.Services;

public class MyDiscussionHub : Hub
{
    private const string URL_TO_SEND_TO = "toto";
    private readonly IHubContext<MyDiscussionHub> _hubContext;
    public MyDiscussionHub(IHubContext<MyDiscussionHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public override Task OnConnectedAsync()
    {
        _hubContext.Clients.All.SendAsync(URL_TO_SEND_TO, "system", $"{Context.ConnectionId} joined the conversation");
        return base.OnConnectedAsync();
    }
    public async Task Send(string name, string message)
    {
        await _hubContext.Clients.All.SendAsync(URL_TO_SEND_TO, name, message);
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _hubContext.Clients.All.SendAsync(URL_TO_SEND_TO, "system", $"{Context.ConnectionId} left the conversation");
        return base.OnDisconnectedAsync(exception);
    }
}