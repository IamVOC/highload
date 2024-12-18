namespace SpaceBattle.Lib;

using Hwdtech;
using System.Text.Json;

public class GameSerializationStrategy : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string gid = (string)args[0];
		IoC.Resolve<object>("Game.Sessions.Scope", gid);
		IDictionary<string, IUObject> ships = IoC.Resolve<IDictionary<string, IUObject>>("Game.Get.Dict");
		Queue<SpaceBattle.Lib.ICommand> queue = IoC.Resolve<Queue<SpaceBattle.Lib.ICommand>>("Game.Get.Queue");
		TimeSpan ts = IoC.Resolve<TimeSpan>("Game.Sessions.TimeSpan");
		Dictionary<string, string> serializedShips = new Dictionary<string, string>();
		string[] serializedQueue = queue
			.Select(cmd => IoC.Resolve<string>("Game.Commands.Serialize", cmd))
			.ToArray();
		foreach(KeyValuePair<string, IUObject> kvp in ships)
		{
			serializedShips.Add(kvp.Key,
					IoC.Resolve<string>("Game.Object.Serialize", kvp.Value));
		}
		string serializedTS = ts.ToString();
		SerializedGameSchema gameDictionary = new SerializedGameSchema
		{
			objects=serializedShips,
			commands=serializedQueue,
			timeSpan=serializedTS
		};
		return JsonSerializer.Serialize(gameDictionary);
	}
}

