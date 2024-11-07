namespace SpaceBattle.Lib;

using System.Collections.Concurrent;

public class RecieverAdapter : IReciever
{
    BlockingCollection<ICommand> queue;

    public RecieverAdapter(BlockingCollection<ICommand> q) => this.queue = q;

    public ICommand Recieve()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}

