namespace MAUICherieApp.Services;

public class MyDiscussion : Hub
{
    private string URL_TO_LISTEN_TO = Constants.API_Url + "/MyDiscussionHub";
    public MyDiscussion()
    {
        Task.Run(async () =>
        {
            await ReadMyMessage();
        });
    }
    // TODO: Receive data
    public async Task ReadMyMessage()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(URL_TO_LISTEN_TO)
            .Build();

        AndroidNotificationManager manager = AndroidNotificationManager.Instance ?? new AndroidNotificationManager();
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected");
            connection.On<string, MySound>("ServiceMessage", (statut, item) =>
            {
                if (statut == "New Sound")
                {
                    manager.Show("Music", statut + " " + item.Name);
                    MessagingCenter.Instance.Send(this, "NewSound");
                }
                else
                {
                    manager.Show("Music", statut + " " + item.Name);
                }
            });
        }
        catch (Exception ex)
        {
            manager.Show("There was an error opening the connection:{0}",
                                  ex.Message);
        }
    }
}