namespace SpaceBattle.Lib;

public class QueueRecieverAdapter : IReciever
{
    Queue<SpaceBattle.Lib.ICommand> queue;

    public QueueRecieverAdapter(Queue<SpaceBattle.Lib.ICommand> q) => this.queue = q;

    public ICommand Recieve()
    {
        return queue.Dequeue();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}
