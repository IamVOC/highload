namespace SpaceBattle.Lib;

using Hwdtech;

public class PushToQueue : IStrategy
{
	public object run_strategy(params object[] args)
	{
		string gameId = (string)args[0];
		ICommand cmd = (ICommand)args[1];

		var queue = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue", gameId);
		return new ActionCommand(() => 
				{
					queue.Enqueue(cmd);
				}); 
	}
}

