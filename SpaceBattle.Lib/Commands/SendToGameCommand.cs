namespace SpaceBattle.Lib;

using Hwdtech;

public class SendToGameCommand : SpaceBattle.Lib.ICommand
{
	private String _gameId;
	private String _obj;

	public SendToGameCommand(String obj, String gameId)
	{
		_obj = obj;
		_gameId = gameId;
	}

	public void Execute()
	{
			
			SpaceBattle.Lib.ICommand cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Commands.Deserialize", _obj);
			object scope = IoC.Resolve<object>("Game.Sessions.Scope", _gameId);
			IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
			IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command", cmd).Execute();
	}
}
