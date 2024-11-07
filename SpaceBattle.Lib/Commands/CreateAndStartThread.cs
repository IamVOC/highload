namespace SpaceBattle.Lib;

using Hwdtech;
using System.Collections.Concurrent;

public class CreateAndStartThreadCommand : SpaceBattle.Lib.ICommand
{
    string id;

    public CreateAndStartThreadCommand(string id)
    {
        this.id = id;
    }

    public void Execute()
    {
        BlockingCollection<SpaceBattle.Lib.ICommand> q = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.SetReciever", id, q).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.SetSender", id, q).Execute();
        MyThread mt = new MyThread(IoC.Resolve<IReciever>("Game.Threads.GetReciever", id));
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.SetThread", id, mt).Execute();
        mt.Execute();
    }
}

