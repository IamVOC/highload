namespace SpaceBattle.Lib;
public class MoveCommand : ICommand
{
    private IMovable target;

    public MoveCommand(IMovable obj)
    {
        this.target = obj;
    }
    public void Execute()
    {
        target.position = target.position + target.velocity;
    }
}

