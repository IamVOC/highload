namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private object[] targets;
    public MacroCommand(object[] objs)
    {
        this.targets = objs;
    }
    public void Execute()
    {
        foreach (ICommand target in targets)
        {
            target.Execute();
        }
    }
}
