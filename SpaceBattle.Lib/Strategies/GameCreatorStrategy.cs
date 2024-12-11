namespace SpaceBattle.Lib;

using Hwdtech;

public class GameCreatorStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string gameId = (string)args[0];
		string scopeId = (string)args[1];

		object gameScope = IoC.Resolve<object>("Game.Session.NewScope", scopeId);
		
		IoC.Resolve<ICommand>("Scope.Register.Dependencies", gameScope).Execute();
		var gameCommand = new GameCommand(gameId);

		return gameCommand;
	}
}
