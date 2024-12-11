namespace SpaceBattle.Lib;

using Hwdtech;

public class CreateSagaCommand : IStrategy {

	public object run_strategy(params object[] args)
    {
		List<Tuple<ICommand, ICommand>> l = new List<Tuple<ICommand, ICommand>>();
		List<ICommand> r = new List<ICommand>();
		
		object[] cmdNames = (object[])args.Take(args.Length - 2).ToArray();
		int pivotid = (int)args[args.Length - 1];
		IUObject uobj = (IUObject)args[args.Length - 2];

		for(int i = 0; i < pivotid; i++){
			var adaptee = IoC.Resolve<object>("Game.Adapter.AdaptForCmd", uobj, cmdNames[i].ToString()!);
			ICommand cmd = IoC.Resolve<ICommand>("Game.Commands.CreateCommand", cmdNames[i].ToString()!, adaptee);

			ICommand compensCmd = IoC.Resolve<ICommand>(
						"Game.Saga.CreateCompensatingCommand", cmdNames[i], adaptee);
			l.Add(new Tuple<ICommand, ICommand>(cmd, compensCmd));
		}
		var pAdaptee = IoC.Resolve<object>("Game.Adapter.AdaptForCmd", uobj, cmdNames[pivotid]);
		ICommand p = IoC.Resolve<ICommand>("Game.Commands.CreateCommand", cmdNames[pivotid], pAdaptee);
		for(int i = pivotid + 1; i < cmdNames.Length; i++) {
			var adaptee = IoC.Resolve<object>("Game.Adapter.AdaptForCmd", uobj, cmdNames[i].ToString()!);
			ICommand cmd = IoC.Resolve<ICommand>("Game.Commands.CreateCommand", cmdNames[i].ToString()!, adaptee);
			r.Add(cmd);
		}
		return new SagaCommand(l, p, r);
	}
}
