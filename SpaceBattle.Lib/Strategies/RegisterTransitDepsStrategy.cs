namespace SpaceBattle.Lib;

using Hwdtech;

public class RegisterTransitDepsStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		return new ActionCommand(() => {
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Serialize", (object[] args) => new GameSerializationStrategy().run_strategy(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Deserialize", (object[] args) => new GameTransitionStrategy().run_strategy(args)).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Deserialize", (object[] args) => new NullObjectStrategy().run_strategy(args)).Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Serialize", (object[] args) => "command").Execute();
			IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Object.Serialize", (object[] args) => "object").Execute();
		});
	}
}
