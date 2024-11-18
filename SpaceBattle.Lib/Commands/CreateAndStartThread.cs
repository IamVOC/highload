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
        BlockingCollection<SpaceBattle.Lib.ICommand> iq = new BlockingCollection<SpaceBattle.Lib.ICommand>();
		BlockingCollection<SpaceBattle.Lib.ICommand> oq = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Inner.SetReciever", id, iq).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Inner.SetSender", id, iq).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Outer.SetReciever", id, oq).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.Outer.SetSender", id, oq).Execute();
        MyThread mt = new MyThread(IoC.Resolve<IReciever>("Game.Threads.Inner.GetReciever", id), 
				IoC.Resolve<IReciever>("Game.Threads.Outer.GetReciever", id));
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Threads.SetThread", id, mt).Execute();
        mt.Execute();
    }
}

