namespace SpaceBattle.Lib;

using Hwdtech;

public class HardStopThreadCommand : SpaceBattle.Lib.ICommand
{
    string id;

    public HardStopThreadCommand(string id)
    {
        this.id = id;
    }
    public void Execute()
    {
        MyThread mt = IoC.Resolve<MyThread>("Game.Threads.GetThread", id);
        if (Thread.CurrentThread == mt.thread)
        {
            mt.Stop();
        }
        else
        {
            throw IoC.Resolve<Exception>("Game.Exceptions.WrongThread");
        }
    }
}

