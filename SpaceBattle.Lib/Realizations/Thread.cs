namespace SpaceBattle.Lib;

using System.Threading;

public class MyThread
{
    public Thread thread;
    IReciever innerQueue;
	IReciever outerQueue;
    bool stop = false;
    Action strategy;

    public MyThread(IReciever innerQueue, IReciever outerQueue)
    {
        this.innerQueue = innerQueue;
		this.outerQueue = outerQueue;
        strategy = () =>
        {
            HandleCommand();
        };
        thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
    }
    internal void HandleCommand()
    {
		if(!innerQueue.isEmpty()) {
			var cmd = innerQueue.Recieve();
			cmd.Execute();
		}
		if(!outerQueue.isEmpty()) {
			var cmd = outerQueue.Recieve();
			cmd.Execute();
		}
    }
    internal void UpdateBehavior(Action newBehavior) => strategy = newBehavior;
    internal void Stop() => stop = true;
    public void Execute() => thread.Start();
}

