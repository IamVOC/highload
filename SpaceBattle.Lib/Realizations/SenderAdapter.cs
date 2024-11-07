namespace SpaceBattle.Lib;

using System.Collections.Concurrent;

public class SenderAdapter : ISender
{
    BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> q) => this.queue = q;

    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}

