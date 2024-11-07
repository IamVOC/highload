namespace SpaceBattle.Lib;

using Hwdtech;

public class GameCommand : ICommand
{
	string gid;
	IReciever q;

	public GameCommand(string gid)
	{
		this.gid = gid;
		this.q = IoC.Resolve<IReciever>("Game.Sessions.GetReciever", gid);
	}

	public void Execute()
	{
		var curset = IoC.Resolve<object>("Game.Sessions.Scope", gid);
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", curset).Execute();
		var tspan = IoC.Resolve<TimeSpan>("Game.Sessions.TimeSpan", gid);
		var stime = DateTime.Now;
		while(DateTime.Now - stime < tspan)
		{
			var cmd = q.Recieve();
			try
			{
				cmd.Execute();
			}
			catch (Exception e)
			{
				IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Exception.ExceptionHandler", cmd, e).Execute();
			}
		}
	}
}

