namespace SpaceBattle.Lib;

using Hwdtech;

public class SoftStopThreadCommand : SpaceBattle.Lib.ICommand
{
    string id;
    Action act;

    public SoftStopThreadCommand(string id, Action act)
    {
        this.id = id;
        this.act = act;
    }
    public void Execute()
    {
        MyThread mt = IoC.Resolve<MyThread>("Game.Threads.GetThread", id);
        if (Thread.CurrentThread == mt.thread)
        {            
			var rc = IoC.Resolve<IReciever>("Game.Threads.GetReciever", id);
            new UpdateBehaviorCommand(mt, () => 
				{ 
					if (rc.isEmpty())
					{ 
						act();
						mt.Stop();
					}
					var cmd = rc.Recieve();
					cmd.Execute();
				}).Execute();
        }
        else
        {
            throw IoC.Resolve<Exception>("Game.Exceptions.WrongThread");
        }
    }
}

