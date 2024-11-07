
namespace SpaceBattle.Lib;

using Hwdtech;

public class GetQueue : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string gameId = (string)args[0];

		if (!IoC.Resolve<IDictionary<string, Queue<ICommand>>>("Game.Get.GameDictionary").TryGetValue(gameId, out Queue<ICommand>? queue))
        {
            throw new Exception();
        }
        else
        {
            return queue;
        }
	}
}

