namespace SpaceBattle.Lib;

using System.Threading;

public class MyThread
{
    public Thread thread;
    IReciever queue;
    bool stop = false;
    Action strategy;

    public MyThread(IReciever queue)
    {
        this.queue = queue;
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
        var cmd = queue.Recieve();
        cmd.Execute();
    }
    internal void UpdateBehavior(Action newBehavior) => strategy = newBehavior;
    internal void Stop() => stop = true;
    public void Execute() => thread.Start();
}

