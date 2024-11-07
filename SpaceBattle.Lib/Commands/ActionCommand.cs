namespace SpaceBattle.Lib;

public class ActionCommand : ICommand
{
    Action action;

    public ActionCommand(Action act)
    {
        action = act;
    }      

    public void Execute()
    {
        action();
    }
}

