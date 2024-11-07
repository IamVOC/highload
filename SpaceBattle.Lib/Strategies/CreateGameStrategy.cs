namespace SpaceBattle.Lib;

using Hwdtech;

public class CreateScopeAndGameStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string gameId = (string)args[0];
		string scopeId = (string)args[1];

		object rootScope = IoC.Resolve<object>("Scopes.Current");
		object gameScope = IoC.Resolve<object>("Game.Session.NewScope", scopeId, rootScope);	


		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Register.Dependencies", (object[] args) => new RegisterDependenciesStrategy().run_strategy(args)).Execute();
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Create.Game", (object[] args) => new CreateGameCommandStrategy().run_strategy(args)).Execute();
		
		IoC.Resolve<ICommand>("Scope.Register.Dependencies", gameScope).Execute();
		var gameCommand = IoC.Resolve<SpaceBattle.Lib.ICommand>("Scope.Create.Game", gameScope);

		return gameCommand;
	}
}

