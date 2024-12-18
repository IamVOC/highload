namespace SpaceBattle.Lib;

using Hwdtech;

public class CatchGameCommand : ICommand
{
    private SerializedGameSchema game;
	private string tid;

    public CatchGameCommand(SerializedGameSchema game, string tid)
    {
		this.game = game;
		this.tid = tid;
    }

    public void Execute()
    {
		SpaceBattle.Lib.ICommand gameCmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Deserialize", this.game);
		ISender sender = IoC.Resolve<ISender>("Game.Threads.Inner.GetSender", tid);
		sender.Send(gameCmd);
    }
}
