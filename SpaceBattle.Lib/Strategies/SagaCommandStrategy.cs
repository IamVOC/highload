namespace SpaceBattle.Lib;

using Hwdtech;

public class CreateSagaCommand : IStrategy {

	public object run_strategy(params object[] args)
    {
		List<Tuple<SpaceBattle.Lib.ICommand, ICommand>> l = new List<Tuple<ICommand, ICommand>>();
		
		var cmdNames = args.Take(args.Length - 1);
		IUObject uobj = (IUObject)args.Last();

		foreach (string cmdName in cmdNames) {
			var adaptee = IoC.Resolve<object>("Game.Adapter.AdaptForCmd", uobj, cmdName);
			ICommand cmd = IoC.Resolve<ICommand>("Game.Commands.CreateCommand", cmdName, adaptee);
			ICommand compensCmd = IoC.Resolve<ICommand>("Game.Saga.CreateCompensatingCommand", cmdName, adaptee);
			l.Add(new Tuple<ICommand, ICommand>(cmd, compensCmd));
		}

		return new SagaCommand(l);
	}
}
