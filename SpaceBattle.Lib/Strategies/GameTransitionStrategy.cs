namespace SpaceBattle.Lib;

using Hwdtech;

public class GameTransitionStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		var serializedGame = (SerializedGameSchema)args[0];
		Dictionary<string, string> serializedDict = serializedGame.objects;
		string[] serializedQueue = serializedGame.commands;
		string serializedTS = serializedGame.timeSpan;
		
		string gid = Guid.NewGuid().ToString();

		SpaceBattle.Lib.ICommand gcmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Session.Create", gid, gid);

		object scope = IoC.Resolve<object>("Game.Sessions.Scope", gid);
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
		IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Sessions.Set.TimeSpan",
				TimeSpan.Parse(serializedTS)).Execute();
		foreach(KeyValuePair<string, string> kvp in serializedDict)
		{
			IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Add.Object",
				   kvp.Key, IoC.Resolve<IUObject>("Game.Object.Deserialize",
				   kvp.Value)).Execute();
		}
		foreach(string cmd in serializedQueue)
		{
			IoC.Resolve<SpaceBattle.Lib.ICommand>("Queue.Enqueue.Command",
					IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Commands.Deserialize", cmd)).Execute();
		}
		return gcmd;
	}
}
