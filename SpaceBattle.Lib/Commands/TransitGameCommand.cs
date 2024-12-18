namespace SpaceBattle.Lib;

using Hwdtech;
using System.Text;
using System.Net.Http;

public class TransitGameCommand : ICommand
{
    private TargetServerSchema server;
	private string gid;

    public TransitGameCommand(TargetServerSchema server, string gid)
    {
		this.server = server;
		this.gid = gid;
    }

    public void Execute()
    {
		string game = IoC.Resolve<string>("Game.Serialize", gid);
		using (HttpClient client = new HttpClient())
        {
			client.DefaultRequestHeaders.Add("Thread-Id", server.threadId);
			HttpContent gameContent = new StringContent(game, Encoding.UTF8, "application/json");
			client.PostAsync(server.serverUrl, gameContent).GetAwaiter().GetResult();
		}
    }
}
