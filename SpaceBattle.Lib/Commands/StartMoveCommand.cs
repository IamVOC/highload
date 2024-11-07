namespace SpaceBattle.Lib;

using Hwdtech;

public class StartMoveCommand : ICommand
{
    private IMoveCommandStartable target;

    public StartMoveCommand(IMoveCommandStartable obj)
    {
        this.target = obj;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.UObject.SetProperty", "velocity", target.uobj, target.velocity).Execute();

        IMovable mo = IoC.Resolve<IMovable>("Game.Adapters.IMovable", target.uobj);

        ICommand mc = IoC.Resolve<ICommand>("Game.Commands.MoveCommand", mo);

        IoC.Resolve<ICommand>("Game.Queue.Push", this.target.queue, mc).Execute();
    }
}

